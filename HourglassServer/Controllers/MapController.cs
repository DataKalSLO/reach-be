using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Map;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer
{
    [DefaultControllerRoute]
    public class MapController : ControllerBase
    {
        private HourglassContext _context;

        public MapController(HourglassContext context)
        {
            _context = context;
        }

        // GET: api/map/[tableName]
        // get FeatureCollection for given table name
        //[HttpGet("{tableName}")]
        //public PointGeometry GetPoint(string tableName)
        //{
        //    try
        //    {
        //        IEnumerable<Location> locations = _context.Location.Where(loc => loc.TableName == tableName);
        //        List<Feature> featureList = new List<Feature>();

        //        var query =
        //            from loc in _context.Location
        //            join point in _context.Point on loc.PointId equals point.Id
        //            select new { Latitude = point.Latitude, Longitude = point.Longitude };

        //        return new PointGeometry(query.First().Longitude, query.First().Latitude);
        //        //foreach (Location loc in locations)
        //        //{
        //        //    featureList.Add(new Feature(loc));
        //        //}

        //        //FeatureCollection features = new FeatureCollection(tableName, featureList);

        //        //return features;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.StackTrace);
        //        return null;
        //    }
        //}

        // GET: api/map/[censusVar]
        // get PolygonFeatureCollection for given census variable description
        [HttpGet("{description}")]
        public PolygonFeatureCollection GetZipCodes(string description)
        {
            try
            {
                string variableName = _context.CensusVariables.Where(v => v.Description.ToUpper().Contains(description.ToUpper())).First().Name;
                Console.WriteLine(variableName);
                string zipCode = _context.CensusData.Where(d => d.VariableName == variableName).First().GeoName;
                Console.WriteLine(zipCode);
                // if geo_type == 'zip' ...
                int[] coordinates = _context.ZipCode.Where(z => z.Zip.ToString() == zipCode).First().Coordinates;
                List<Point> points = new List<Point>();
                foreach (int pt in coordinates)
                {
                    Point point = _context.Point.Where(p => p.Id == pt).First();
                    points.Add(point);
                }
                PolygonFeature geom = new PolygonFeature(points);
                List<PolygonFeature> features = new List<PolygonFeature>() { geom };
                PolygonFeatureCollection collection = new PolygonFeatureCollection(features);
                return collection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        public string Post()
        {
            return "Creating Maps is not yet implemented.";
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating Maps is not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete()
        {
            return "Deleting Maps is not yet implemented.";
        }
    }
}
