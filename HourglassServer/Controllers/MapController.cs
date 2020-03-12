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
                // finding variable name of given census description
                string variableName = _context.CensusVariables.Where(v => v.Description.ToUpper().Contains(description.ToUpper())).First().Name;

                // getting all rows in CensusData with variableName
                IEnumerable<CensusData> cData = _context.CensusData.Where(d => d.VariableName == variableName);

                // creating empty list of PolygonFeatures
                List<PolygonFeature> features = new List<PolygonFeature>();

                // iterating through CensusData rows
                foreach (CensusData data in cData)
                {
                    // if geo type is zip, go through ZipCode table
                    if (data.GeoType == GeoType.zip)
                    {
                        // getting all rows in ZipCode with specified zipcode name
                        IEnumerable<ZipCode> zips = _context.ZipCode
                            .Where(z => z.Zip.ToString() == data.GeoName);

                        // iterating through zipcodes (should only be one ?)
                        foreach (ZipCode zip in zips)
                        {
                            // iterating through point ids in zip coordinates list
                            // this should be using foreign key
                            List<Point> points = new List<Point>();
                            foreach (int pt in zip.Coordinates)
                            {
                                Point point = _context.Point.Where(p => p.Id == pt).First();
                                points.Add(point);
                            }
                            // creating PolygonFeature using new list of points and name of location
                            PolygonFeature geom = new PolygonFeature(points, data.GeoName);
                            features.Add(geom);
                        }

                    }
                    // TODO: add functionality for city, county
                }
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
