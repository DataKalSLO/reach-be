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

        private async Task<List<Point>> GetPoints(string geoName)
        {
            // all rows in Area with specified zipcode name
            var points = from area in _context.Area
                        join point in _context.Point
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

                // getting all rows in CensusData with variableName
                IEnumerable<CensusData> cData = await _context.CensusData.Where(d => d.VariableName == variableName).ToListAsync(); ;

                // creating empty list of PolygonFeatures
                List<PolygonFeature> features = new List<PolygonFeature>();

                // iterating through CensusData rows
                foreach (CensusData data in cData)
                {
                    List<Point> points = await GetPoints(data.GeoName);
                    PolygonFeature geom = new PolygonFeature(points, data.GeoName, data.Value);
                    features.Add(geom);
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
