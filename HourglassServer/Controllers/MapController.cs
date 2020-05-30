using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.Maps;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public class metaCensus
        {
            public string tableName { get; set; }
            public string censusDesc { get; set; }
            public string geoType { get; set; }

        }

        // GET: api/map/tableNames
        [HttpGet("tableNames")]
        public ActionResult<List<metaCensus>> ReadableCensusNames()
        {
            try
            {
                Dictionary<string, string> censusNameDesc = new Dictionary<string, string>();
                List<metaCensus> metaCensusList = new List<metaCensus>();
                var censusVariables = from census in _context.CensusVariables
                                      select census;
                var metaData = from meta in _context.DatasetMetaData
                               select meta;

                foreach (CensusVariables censusVar in censusVariables)
                {
                    censusNameDesc.Add(censusVar.Name.ToLower(), censusVar.Description);
                }

                foreach (DatasetMetaData data in metaData)
                {
                    var temp = new metaCensus();
                    temp.geoType = data.GeoType;
                    if (censusNameDesc.Keys.Contains(data.TableName))
                    {
                        
                        temp.tableName = data.TableName;
                        temp.censusDesc = (censusNameDesc[data.TableName]);
                        metaCensusList.Add(temp);
                    }
                    else
                    {
                        temp.tableName = data.TableName;
                        metaCensusList.Add(temp);
                    }
                }
                return metaCensusList;
            }
            catch (Exception e)
            {
                return BadRequest(
                    new ExceptionMessageContent()
                    {
                        Error = "Table does not exist",
                        Message = e.ToString()
                    });
            }
        }

        // GET: api/map/[censusVar]
        // get PolygonFeatureCollection for given census variable description
        // get FC based on given data table name
        [HttpGet("{tableName}")]
        public ActionResult<PolygonFeatureCollection> GetZipCodes(string tableName)
        {
            try
            {
                MapLocationRetriever retriever = new MapLocationRetriever();
                PolygonFeatureCollection collection =
                    retriever.GetPolygonFeatureCollection(tableName, _context, _dataContext);
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
