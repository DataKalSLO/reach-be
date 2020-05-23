using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.MetadataModel;
using Microsoft.Extensions.Caching.Memory;

namespace HourglassServer.Data.DataManipulation.MetadataOperations
{
    public static class MetadataOperations
    {
        public static async Task<List<MetadataApplicationModel>> GetMetadata(this IMemoryCache cache, HourglassContext db)
        {
            List<MetadataApplicationModel> metadata = new List<MetadataApplicationModel>();

            // Look for the cache key
            if (!cache.TryGetValue<List<MetadataApplicationModel>>(CacheKeys.MetadataKey, out metadata))
            {
                // Update the metadata with a fresh call to the database
                metadata = await UpdateMetadataCache(db, cache);
            }

            return metadata;
        }

        public static void ExpireMetadataCache(this IMemoryCache cache)
        {
            cache.Remove(CacheKeys.MetadataKey);
        }

        private static async Task<List<MetadataApplicationModel>> UpdateMetadataCache(HourglassContext db, IMemoryCache cache)
        {
            List<MetadataApplicationModel> metadata = new List<MetadataApplicationModel>();

            // Update the metadata with a fresh call to the database
            metadata = await GetMetadataFromDb(db);

            // Set options on the cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Set metadata cache to expire every hour
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            // Save the new metadata value in the cache
            cache.Set<List<MetadataApplicationModel>>(CacheKeys.MetadataKey, metadata, cacheEntryOptions);

            return metadata;
        }

        private static async Task<List<MetadataApplicationModel>> GetMetadataFromDb(HourglassContext db)
        {
            List<DatasetMetaData> datasetMetadata = await db.DatasetMetaData.ToListAsync();
            List<MetadataApplicationModel> metadata = new List<MetadataApplicationModel>();

            foreach (DatasetMetaData tableMetadata in datasetMetadata)
            {
                metadata.Add(new MetadataApplicationModel
                {
                    TableName = tableMetadata.TableName,
                    ColumnNames = tableMetadata.ColumnNames,
                    DataTypes = tableMetadata.DataTypes,
                    GeoType = tableMetadata.GeoType
                });
            }

            return metadata;
        }
    }
}
