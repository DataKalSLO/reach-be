using System;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using HourglassServer.Data.Application.MetadataModel;
using HourglassServer.Data.DataManipulation.MetadataOperations;

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
        private HourglassContext _dbContext;
        private IConfiguration _config;
        private IMemoryCache _cache;
        private readonly ILogger _logger;

        public DatasetDbContext(DbContextOptions<DatasetDbContext> options,
                HourglassContext context,
                IConfiguration config,
                IMemoryCache cache,
                ILogger<DatasetDbContext> logger) : base(options)
        {
            _dbContext = context;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        private NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(_config.GetConnectionString("HourglassDatabase"));
        }

        public async Task<DataSet> getDataSet(string tableName, string[] columns)
        {
            QueryFormatUtil queryUtil = new QueryFormatUtil();

            // Get a copy of the metadata from the cache
            List<MetadataApplicationModel> metadata = await _cache.GetMetadata(_dbContext);

            // Invoke query util to format a full dataset select query using the table name
            if (!queryUtil.formatTableQuery(tableName, metadata))
            {
                // The tableName provided doesn't exist in the database
                _logger.LogError(
                    String.Format("{0}: Provided table {1} does not exist in the metadata.",
                    nameof(getDataSet),
                    tableName));

                throw new TableNotFoundException(queryUtil.Error);
            }
            if (!queryUtil.createQuery(metadata.Find(x => x.TableName == tableName), columns))
            {
                _logger.LogError(
                    String.Format("{0}: Provided table {1} does not contain the column {2}.",
                        nameof(getDataSet),
                        tableName,
                        queryUtil.BadColumn));

                throw new ColumnNotFoundException(queryUtil.Error);
            }
            // Get the specified data set using the query
            try
            {
                List<Object[]> datasetRows = new List<Object[]>();
                var conn = getConnection();
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand(queryUtil.getQuery(), conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        Object[] values = new Object[reader.FieldCount];
                        reader.GetValues(values);
                        datasetRows.Add(values);
                    }
                conn.Close();
                return new DataSet
                {
                    Data = datasetRows
                };
            }
            catch (Npgsql.PostgresException e)
            {
                // SqlState 42P01 is an error code for table names that do not exist in the database
                // A stale metadata cache would be the reason for this error
                if (e.SqlState == "42P01")
                {
                    _logger.LogError(
                        String.Format("{0}: Bad SQL query due to stale cache. Table {1} does not exist in database.",
                        nameof(getDataSet),
                        tableName));

                    // Expire the cache and throw an exception
                    _logger.LogDebug(String.Format("{0}: Expiring stale cache", nameof(getDataSet)));
                    _cache.ExpireMetadataCache();

                    throw new StaleRequestException(String.Format("Table no longer exists in database: {0}", tableName));
                }

                // Other Postgres exception occured
                throw e;
            }
        }
    }
}