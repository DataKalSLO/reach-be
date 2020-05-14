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
        private MapDbContext _dataContext;

        public MapController(HourglassContext context, MapDbContext dataContext)
        {
            _context = context;
            _dataContext = dataContext;
        }

        private List<Point> GetPoints(string geoName)
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
        // get FC based on given data table name
        [HttpGet("{tableName}")]
        public ActionResult<PolygonFeatureCollection> GetZipCodes(string tableName)
        {
            try
            {
                // search for table name in metadata table
                var metaData = from meta in _context.DatasetMetaData
                               where meta.TableName == tableName
                               select meta;

                // get location data from table
                List<LocationData> dataSet = _dataContext.getLocationData(tableName).Result;
                List<PolygonFeature> features = new List<PolygonFeature>();

                // for each row of location data, get the latitude, longitude pairs
                foreach (LocationData row in dataSet)
                {
                    List<Point> points = GetPoints(row.GeoName);
                    // create feature from list of points
                    PolygonFeature geom = new PolygonFeature(points, row.GeoName, row.Value);
                    features.Add(geom);
                }

                PolygonFeatureCollection collection = new PolygonFeatureCollection(features);
                return collection;
            }
            catch (Exception e)
            {
                return BadRequest(
                        new ExceptionMessageContent()
                        {
                            Error = "Table does not exist",
                            Message = e.ToString()
                        }); ;
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
