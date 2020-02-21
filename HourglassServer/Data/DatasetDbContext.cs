using System;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

/**
 *----------------------------------------
 * Data Base Context class 
 *----------------------------------------
 * This class is used for configuring 
 * and connecting the database.
 * This class provides the following methods:
 * -OnConfiguring
 *    -configures the database 
 *     using the connnection string
 * -getConnection
 *    -creates a NpgsqlConnection using
 *     the connection string
 *-getAllDatasetMetadata
 *   -returns the meta data for the tables 
 *    in the database
 *-getDataSet 
 *   -returns the requested dataset
 *   -uses matchedName and formatString 
 *    to prevent sql injection
 *----------------------------------------
 */

namespace HourglassServer.Data
{
    public class DatasetDbContext : DbContext
    {
        private IConfiguration _config;
        private List<DatasetMetadata> metadata;

        public DatasetDbContext(DbContextOptions<DatasetDbContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("HourglassDatabase"));
            }
        }

        private NpgsqlConnection getConnection() {
            return new NpgsqlConnection(_config.GetConnectionString("HourglassDatabase"));
        }

        //Helper method for checking the metadata 
        private bool matchedName(string tableName){
            foreach (DatasetMetadata md in metadata){
                if (md.TableName.Equals(tableName)){
                    return true;
                }
            }
            return false;
        }

        //Helper Method used for formatting a sql query string
        private async Task<string> formatString(string tableName){
            // clean SQL tableName param by checking if table exists in db
            if (metadata == null || !matchedName(tableName)) {
                // update local metadata cache
                await getAllDatasetMetadata();
            
                if (!matchedName(tableName)) {
                    throw new KeyNotFoundException(
                        String.Format("TableName: {0} not found.", tableName));
                }
            }
            return String.Format("SELECT * FROM {0}", tableName);
        }

        public async Task<List<DataSets>> getDataSet(string tableName){
            //TODO: connect to DB and return requested dataset
            //formatString(tableName).Result;
            //Test of concept code. Will NOT make it into production
            List<DataSets> datasets = new List<DataSets>();
            DataSets data;
            var conn = getConnection();
            await conn.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT * FROM datasetmetadata", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            while (await reader.ReadAsync()) {
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);
                data = new DataSets{
                    Row = values
                };
                datasets.Add(data);
            }
            return datasets;
        }

        public async Task<List<DatasetMetadata>> getAllDatasetMetadata() {

            List<DatasetMetadata> dsMetadata = new List<DatasetMetadata>();
            DatasetMetadata meta_data;
            string tableName;
            string[] columnNames;
            string[] columnTypes;
            var conn = getConnection();
            await conn.OpenAsync();

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT * FROM datasetmetadata", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            while (await reader.ReadAsync()) {
                tableName = reader.GetString(0);
                columnNames = (string[])reader.GetValue(1);
                columnTypes = (string[])reader.GetValue(2);

                meta_data = new DatasetMetadata{
                    TableName = tableName,
                    ColumnNames = columnNames,
                    ColumnTypes = columnTypes
                };
                dsMetadata.Add(meta_data);
            }
            // cache a copy locally
            metadata = dsMetadata;
            return dsMetadata;
        }
    }
}
