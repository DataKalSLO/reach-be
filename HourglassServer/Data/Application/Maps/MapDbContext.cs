using System;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class MapDbContext : DbContext
    {
        private IConfiguration _config;

        private IMemoryCache _cache;

        private readonly ILogger _logger;

        public MapDbContext(DbContextOptions<MapDbContext> options,
                IConfiguration config,
                IMemoryCache cache,
                ILogger<MapDbContext> logger) : base(options)
        {

            _config = config;
            _cache = cache;
            _logger = logger;
        }

        private NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(_config.GetConnectionString("HourglassDatabase"));
        }

        private int convertValueType(decimal value)
        {
            return (int)(value * 100);
        }

        public async Task<List<object[]>> GetColumns(string tableName, string valueType)
        {
            List<object[]> datasetRows = new List<object[]>();
            var conn = getConnection();
            await conn.OpenAsync();

            var sql = "SELECT * from datasets." + tableName;
            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    object[] values = new object[reader.FieldCount];
                    reader.GetValues(values);
                    datasetRows.Add(values);
                }
            conn.Close();
            return datasetRows;
        }

        // checking for injection must be done before calling this function
        // including checking that DataSetMetaData includes given tableName
        public async Task<List<LocationData>> GetLocationData(string tableName, string valueType)
        {
            var conn = getConnection();
            await conn.OpenAsync();
            string geoName;
            int? value;

            LocationData locationData;
            List<LocationData> dataRows = new List<LocationData>();

            // prepared statement not working here
            var sql = "SELECT geo_name, value from datasets." + tableName;
            using var cmd = new NpgsqlCommand(sql, conn);
            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        geoName = reader.GetString(0);
                        if (valueType == "decimal")
                        {
                            value = convertValueType((decimal)reader.GetValue(1));
                        }
                        else
                        {
                            value = (int?)reader.GetValue(1);
                        }

                        locationData = new LocationData
                        {
                            GeoName = geoName,
                            Value = value
                        };
                        dataRows.Add(locationData);
                    }
                return dataRows;
            }
            catch (PostgresException e)
            {
                // SqlState 42P01 is an error code for table names that do not exist in the database
                // A stale metadata cache would be the reason for this error
                if (e.SqlState == "42P01")
                {
                    _logger.LogError(
                        string.Format("{0}: Bad SQL query due to stale cache. Table {1} does not exist in database.",
                        nameof(GetLocationData),
                        tableName));

                    // Expire the cache and throw an exception
                    _logger.LogDebug(string.Format("{0}: Expiring stale cache", nameof(GetLocationData)));
                    _cache.Remove(CacheKeys.MetadataKey);

                    throw new StaleRequestException(string.Format("Table no longer exists in database: {0}", tableName));
                }

                // Other Postgres exception occured
                throw e;
            }
        }

    }
}