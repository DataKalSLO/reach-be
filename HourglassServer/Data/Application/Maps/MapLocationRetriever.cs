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

        private List<Point> GetPoints(string geoName, HourglassContext context)
        {
            // all rows in Area with specified zipcode name
            var points = from area in context.Area
                         join point in context.Point
                             on area.PointId equals point.Id
                         where area.Name == geoName
                         select point;

            List<Point> pts = new List<Point>();
            foreach (Point pt in points)
            {
                pts.Add(pt);
            }
            return pts;
        }

        public PolygonFeatureCollection GetPolygonFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {
            
            // search for table name in metadata table
            // if bad table name, Exception is thrown
            var metaData = from meta in context.DatasetMetaData
                            where meta.TableName == tableName
                            select meta;

            // get location data from table
            List<LocationData> dataSet = dataContext.getLocationData(tableName).Result;
            List<PolygonFeature> features = new List<PolygonFeature>();

            // for each row of location data, get the latitude, longitude pairs
            foreach (LocationData row in dataSet)
            {
                List<Point> points = GetPoints(row.GeoName, context);
                // create feature from list of points
                PolygonFeature geom = new PolygonFeature(points, row.GeoName, row.Value);
                features.Add(geom);
            }

            PolygonFeatureCollection collection = new PolygonFeatureCollection(features);
            collection.Name = tableName;
            return collection;
            
        }
    }
}
