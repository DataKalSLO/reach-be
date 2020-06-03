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

        private List<int> GetHeatMapPolygon(string geoName, HourglassContext context)
        {
            // all rows in Area with specified zipcode name
            var polygons = (from area in context.Area
                         join polygon in context.Polygon
                             on area.PolygonId equals polygon.Id
                         where area.Name == geoName
                         select polygon.Id).Distinct();
            Console.WriteLine("Getting polygons");

            List<int> polygonIds = new List<int>();
            foreach (int polygonId in polygons)
            {
                Console.WriteLine(polygonId);
                polygonIds.Add(polygonId);
            }
            return polygonIds;
        }

        private List<Point> GetHeatMapPoints(int polygonId, HourglassContext context)
        {
            // all rows in Area with specified zipcode name
            var points = from polygon in context.Polygon
                           join point in context.Point
                           on polygon.PointId equals point.Id
                           where polygon.Id == polygonId
                           select point;
            Console.WriteLine("Points retrieved from polygonId: " + polygonId);
            List<Point> pts = new List<Point>();
            foreach (Point pt in points)
            {
                pts.Add(pt);
            }
            return pts;
        }

        private List<List<Point>> GetMultiPolygonPoints(List<int> polygonIds, HourglassContext context)
        {
            List<List<Point>> points = new List<List<Point>>();
            foreach (int polygonId in polygonIds)
            {
                List<Point> polygonPoints = GetHeatMapPoints(polygonId, context);
                points.Add(polygonPoints);
            }
            return points;
        }

        private PointGeometry GetMarkerPoints(string geoName, HourglassContext context)
        {
            var points = from location in context.Location
                         join point in context.Point
                         on location.PointId equals point.Id
                         where location.Name == geoName
                         select point;

            PointGeometry pt = new PointGeometry(points.First().Longitude, points.First().Latitude);
            return pt;
        }

        public PolygonFeatureCollection GetPolygonFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {
            
            // search for table name in metadata table
            // if bad table name, Exception is thrown
            var metaData = from meta in context.DatasetMetaData
                            where meta.TableName == tableName
                            && meta.GeoType == "area"
                            select meta;

            // get location data from table
            List<LocationData> dataSet = dataContext.getLocationData(tableName, "int").Result;
            List<object> features = new List<object>();

            // for each row of location data, get the latitude, longitude pairs
            foreach (LocationData row in dataSet)
            {
                Console.WriteLine("GEO_NAME" + row.GeoName);
                GeoArea area = context.GeoArea.Where(g => g.Name == row.GeoName).First();

                PolygonFeature geom = new PolygonFeature(area.Geometry, row.GeoName, row.Value);
                features.Add(geom);
               // List<int> polygonIds = GetHeatMapPolygon(row.GeoName, context);

               //// List<Polygon> polygons = dataContext.getPolygons(row.GeoName).Result;

               // // normal Polygon
               // if (polygonIds.Count() == 1)
               // {
               //     List<Point> points = GetHeatMapPoints(polygonIds.First(), context);
               //     PolygonFeature geom = new PolygonFeature(points, row.GeoName, row.Value);
               //     Console.WriteLine(geom);
               //     features.Add(geom);
               // }
               // else // MultiPolygon
               // {
               //     List<List<Point>> polygonPoints = GetMultiPolygonPoints(polygonIds, context);
               //     MultiPolygonFeature feat = new MultiPolygonFeature(polygonPoints, row.GeoName, row.Value);
               //     features.Add(feat);
               // }

            }

            PolygonFeatureCollection collection = new PolygonFeatureCollection(features);
            collection.Name = tableName;
            return collection;
            
        }

        public FeatureCollection GetPointFeatureCollection(string tableName, HourglassContext context, MapDbContext dataContext)
        {
            var metaData = from meta in context.DatasetMetaData
                          where meta.TableName == tableName
                          && meta.GeoType == "location"
                          select meta;
            List<LocationData> locationData = dataContext.getLocationData(tableName, "decimal").Result;
            List<Feature> features = new List<Feature>();

            foreach (LocationData row in locationData)
            {
                Feature feature = new Feature(GetMarkerPoints(row.GeoName, context), row.GeoName, row.Value);
                features.Add(feature);
            }
           
            FeatureCollection collection = new FeatureCollection(tableName, features);
            return collection;
        }
    }
}
