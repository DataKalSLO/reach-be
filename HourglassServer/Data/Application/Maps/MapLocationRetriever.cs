using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.Maps;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Data.Application.Maps
{
    public class MapLocationRetriever
    {
        public MapLocationRetriever()
        {
        }

        public PolygonFeatureCollection GetPolygonFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {

            // search for table name in metadata table
            // if bad table name, Exception is thrown
            var metaData = context.DatasetMetaData.Where(md => md.TableName == tableName && md.GeoType == "area");

            // get location data from table
            List<LocationData> dataSet = dataContext.getLocationData(tableName, "int").Result;
            List<object> features = new List<object>();

            // for each row of location data, get the latitude, longitude pairs
            foreach (LocationData row in dataSet)
            {
                GeoArea area = context.GeoArea.Where(g => g.Name == row.GeoName).First();

                PolygonFeature geom = new PolygonFeature(area.Geometry, row.GeoName, row.Value);
                features.Add(geom);
            }

            PolygonFeatureCollection collection = new PolygonFeatureCollection(features);
            collection.Name = tableName;
            return collection;
            
        }

        public FeatureCollection GetPointFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {
            var metaData = context.DatasetMetaData.Where(md => md.TableName == tableName && md.GeoType == "location");

            List<LocationData> locationData = dataContext.getLocationData(tableName, "decimal").Result;
            List<Feature> features = new List<Feature>();

            foreach (LocationData row in locationData)
            {
                GeoLocation loc = context.GeoLocation.Where(g => g.Name == row.GeoName).First();
                Feature feature = new Feature(loc.Geometry, row.GeoName, row.Value);
                features.Add(feature);
            }
           
            FeatureCollection collection = new FeatureCollection(tableName, features);
            return collection;
        }
    }
}
