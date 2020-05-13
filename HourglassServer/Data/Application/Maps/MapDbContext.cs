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

        // checking for injection must be done before calling this function
        // including checking that DataSetMetaData includes given tableName
        public async Task<List<LocationData>> getLocationData(string tableName)
        {
            var conn = getConnection();
            await conn.OpenAsync();
            string geoName;
            string geoType;
            int? value;

            LocationData locationData;
            List<LocationData> dataRows = new List<LocationData>();

            // prepared statement not working here
            var sql = "SELECT geo_name, geo_type, value from datasets." + tableName;
            using var cmd = new NpgsqlCommand(sql, conn);
            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        geoName = reader.GetString(0);
                        geoType = reader.GetString(1);
                        value = (int?)reader.GetValue(2);

                        locationData = new LocationData
                        {
                            GeoName = geoName,
                            GeoType = geoType,
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
                        nameof(getLocationData),
                        tableName));

                    // Expire the cache and throw an exception
                    _logger.LogDebug(string.Format("{0}: Expiring stale cache", nameof(getLocationData)));
                    _cache.Remove(CacheKeys.MetadataKey);

                    throw new StaleRequestException(string.Format("Table no longer exists in database: {0}", tableName));
                }

                // Other Postgres exception occured
                throw e;
            }
        }
    }
}