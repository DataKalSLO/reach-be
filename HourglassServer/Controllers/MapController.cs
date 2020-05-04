using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.Maps;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class MapController : Controller
    {
        private HourglassContext _context;

        public MapController(HourglassContext context)
        {
            _context = context;
        }

        // GET: api/map/[censusVar]
        // get PolygonFeatureCollection for given census variable description
        [HttpGet("{description}")]
        public async Task<PolygonFeatureCollection> GetZipCodes(string description)
        {
            try
            {
                // finding variable name of given census description
                var variables = await _context.CensusVariables.Where(v => v.Description.ToUpper().Contains(description.ToUpper())).ToListAsync();
                string variableName = variables[0].Name;
                Console.WriteLine("Variable Name: " + variableName);

                // getting all rows in CensusData with variableName
                IEnumerable<CensusData> cData = await _context.CensusData.Where(d => d.VariableName == variableName).ToListAsync(); ;
                //var asyncCData = await (from c in _context.CensusData
                //                   where c.VariableName == variableName
                //                   select c).ToListAsync();

                Console.WriteLine("GEONAME: " + cData.ToList()[0].GeoName);

                // creating empty list of PolygonFeatures
                List<PolygonFeature> features = new List<PolygonFeature>();

                // iterating through CensusData rows
                foreach (CensusData data in cData)
                {
                    Console.WriteLine(data.GeoType.ToString());
                    // if geo type is zip, go through ZipCode table
                    if (data.GeoType.Equals("zip"))
                    {
                        // getting all rows in ZipCode with specified zipcode name
                        IEnumerable<ZipCode> zips = await _context.ZipCode
                            .Where(z => z.Zip.ToString() == data.GeoName).ToListAsync();

                        // iterating through zipcodes (should only be one ?)
                        foreach (ZipCode zip in zips)
                        {
                            // iterating through point ids in zip coordinates list
                            // this should be using foreign key
                            List<Point> points = new List<Point>();
                            foreach (int pt in zip.Coordinates)
                            {
                                // TODO: this takes way too long
                                Point point = await _context.Point.Where(p => p.Id == pt).FirstAsync();
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
