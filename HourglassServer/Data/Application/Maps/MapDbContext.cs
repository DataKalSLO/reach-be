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

        public async Task<List<Polygon>> getPolygons(string geoName)
        {
            var conn = getConnection();
            await conn.OpenAsync();
            int id;
            int pointId;

            Polygon polygon;
            List<Polygon> polygons = new List<Polygon>();
            // select * from polygon p join area a on a.polygon_id=p.id where a.Name='93422'
            var sql = "SELECT p.id, p.point_id from polygon p join area a on a.polygon_id=p.id where a.name='"
                + geoName + "'";
            using var cmd = new NpgsqlCommand(sql, conn);
            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        id = (int)reader.GetValue(0);
                        pointId = (int)reader.GetValue(1);
                        
                        polygon = new Polygon
                        {
                            Id = id,
                            PointId = pointId
                        };
                        polygons.Add(polygon);
                    }
                return polygons;
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
                        geoName));

                    // Expire the cache and throw an exception
                    _logger.LogDebug(string.Format("{0}: Expiring stale cache", nameof(getLocationData)));
                    _cache.Remove(CacheKeys.MetadataKey);

                    throw new StaleRequestException(string.Format("Table no longer exists in database: {0}", geoName));
                }

                // Other Postgres exception occured
                throw e;
            }
        }

        // checking for injection must be done before calling this function
        // including checking that DataSetMetaData includes given tableName
        public async Task<List<LocationData>> getLocationData(string tableName, string valueType)
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

        public async Task<List<Feature>> getPoints(string tableName)
        {
            var conn = getConnection();
            await conn.OpenAsync();
            decimal longitude;
            decimal latitude;
            string name;

            PointGeometry point;
            Feature feature;
            List<Feature> pointRows = new List<Feature>();

            // prepared statement not working here
            var sql = "SELECT latitude, longitude, name from datasets." + tableName;
            using var cmd = new NpgsqlCommand(sql, conn);
            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        latitude = (decimal)reader.GetValue(0);
                        longitude = (decimal)reader.GetValue(1);
                        name = reader.GetString(2);

                        point = new PointGeometry(longitude, latitude);
                        feature = new Feature(point, name, 0);
                        pointRows.Add(feature);
                    }
                return pointRows;
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