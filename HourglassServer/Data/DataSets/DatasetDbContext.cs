using System;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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

        private IMemoryCache _cache;

        private readonly ILogger _logger;

        public DatasetDbContext(DbContextOptions<DatasetDbContext> options, 
                IConfiguration config,
                IMemoryCache cache,
                ILogger<DatasetDbContext> logger): base(options) {

            _config = config;
            _cache = cache;
            _logger = logger;
        }
        
        private NpgsqlConnection getConnection() {
            return new NpgsqlConnection(_config.GetConnectionString("HourglassDatabase"));
        }

        public async Task<DataSet> getDataSet(string tableName) {
            QueryFormatUtil queryUtil = new QueryFormatUtil();

            // Get a copy of the metadata from the cache
            Task<List<DatasetMetadata>> getMetadataCache = getDatasetMetadata();
            List<DatasetMetadata> metadata = await getMetadataCache;

            // Invoke query util to format a full dataset select query using the table name
            if (!queryUtil.formatSelectFullDatasetQuery(tableName, metadata)) {
                // The tableName provided doesn't exist in the database
                _logger.LogError(
                    String.Format("{0}: Provided table {1} does not exist in the metadata.",
                    nameof(getDataSet), 
                    tableName));
                
                throw new TableNotFoundException(queryUtil.Error);
            }
            
            // Get the specified data set using the query
            try {
                List<Object[]> datasetRows = new List<Object[]>();
                var conn = getConnection();
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand(queryUtil.getQuery(), conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync()) {
                    Object[] values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    datasetRows.Add(values);
                }
                return new DataSet{
                    Data = datasetRows
                };
            } catch (Npgsql.PostgresException e) {
                // SqlState 42P01 is an error code for table names that do not exist in the database
                // A stale metadata cache would be the reason for this error
                if (e.SqlState == "42P01") {
                    _logger.LogError(
                        String.Format("{0}: Bad SQL query due to stale cache. Table {1} does not exist in database.",
                        nameof(getDataSet), 
                        tableName));
                    
                    // Expire the cache and throw an exception
                    _logger.LogDebug(String.Format("{0}: Expiring stale cache", nameof(getDataSet)));
                    _cache.Remove(CacheKeys.MetadataKey);

                    throw new StaleRequestException(String.Format("Table no longer exists in database: {0}", tableName));
                }

                // Other Postgres exception occured
                throw e;
            }
        }

        // Gets the dataset metadata from the internal memory cache
        // Fetches a fresh copy on expiration
        public async Task<List<DatasetMetadata>> getDatasetMetadata() {
            List<DatasetMetadata> dsMetadata = new List<DatasetMetadata>();
            
            // Look for the cache key
            if (!_cache.TryGetValue<List<DatasetMetadata>>(CacheKeys.MetadataKey, out dsMetadata)) {
                // The metadata key does not exist in the cache
                _logger.LogDebug($"{nameof(getDatasetMetadata)}: No metadata exists in cache.");

                // Update the metadata with a fresh call to the database
                Task<List<DatasetMetadata>> updateMetadata = getDatasetMetadataFromDB();
                dsMetadata = await updateMetadata;

                // Set options on the cache
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Set metadata cache to expire every hour
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                // Save the new metadata value in the cache
                _cache.Set<List<DatasetMetadata>>(CacheKeys.MetadataKey, dsMetadata, cacheEntryOptions);
                _logger.LogInformation($"{nameof(getDatasetMetadata)}: Succesfully updated cached metadata.");
            }

            return dsMetadata;
        }

        // Fetch a copy of the metadata from the database
        //
        // Note: This function should only be used when updating the cache.
        // All other usages of metadata should use the cached value from getDatasetMetadata()
        private async Task<List<DatasetMetadata>> getDatasetMetadataFromDB() {

            List<DatasetMetadata> dsMetadata = new List<DatasetMetadata>();
            DatasetMetadata meta_data;
            string tableName;
            string[] columnNames;
            string[] columnTypes;
            var conn = getConnection();
            await conn.OpenAsync();

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand(QueryFormatUtil.MetadataQuery, conn))
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

            _logger.LogDebug($"{nameof(getDatasetMetadataFromDB)}: Fetched new metadata from database.");
            return dsMetadata;
        }
    }
}
