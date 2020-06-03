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
            var metaData = context.DatasetMetaData.Where(md => md.TableName == tableName && md.GeoType == "area").First();

            string[] columns = metaData.ColumnNames;
            List<object[]> values = dataContext.GetColumns(tableName, "int").Result;
           
            int nameId = columns.ToList().IndexOf("geo_name");

            // get location data from table
        //    List<LocationData> dataSet = dataContext.GetLocationData(tableName, "int").Result;
            
            List<object> features = new List<object>();

            // for each row of location data, get the latitude, longitude pairs
            foreach (object[] row in values)
            {
                Dictionary<string, object> dataColumnValuePairs = new Dictionary<string, object>();
                string geoName = (string)row[nameId];
                GeoArea area = context.GeoArea.Where(g => g.Name == geoName).First();

                for (int i = 0; i < columns.Length; i++)
                {
                    string colName = columns[i];
                    object value = row[i];
                    dataColumnValuePairs.Add(colName, value);
                }

                PolygonFeature geom = new PolygonFeature(area.Geometry,
                    dataColumnValuePairs);
                features.Add(geom);
            }

            PolygonFeatureCollection collection = new PolygonFeatureCollection(features)
            {
                Name = tableName
            };
            return collection;
            
        }

        public FeatureCollection GetPointFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {
            var metaData = context.DatasetMetaData.Where(md => md.TableName == tableName && md.GeoType == "location");

            string[] columns = metaData.First().ColumnNames;
            List<object[]> values = dataContext.GetColumns(tableName, "int").Result;
            int nameId = columns.ToList().IndexOf("geo_name");

            //  List<LocationData> locationData = dataContext.GetLocationData(tableName, "decimal").Result;
            List<Feature> features = new List<Feature>();

            foreach (object[] row in values)
            {
                Dictionary<string, object> dataColumnValuePairs = new Dictionary<string, object>();
                string geoName = (string)row[nameId];
                GeoLocation loc = context.GeoLocation.Where(g => g.Name == geoName).First();
                for (int i = 0; i < columns.Length; i++)
                {
                    string colName = columns[i];
                    object value = row[i];
                    dataColumnValuePairs.Add(colName, value);
                }
                Feature feature = new Feature(loc.Geometry, dataColumnValuePairs);
                features.Add(feature);
            }
           
            FeatureCollection collection = new FeatureCollection(tableName, features);
            return collection;
        }
    }
}
