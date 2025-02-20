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
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
   [DefaultControllerRoute]
   public class MapController : Controller
   {
      private HourglassContext _context;
      private DatasetDbContext _dataContext;

      public MapController(HourglassContext context, DatasetDbContext dataContext)
      {
         _context = context;
         _dataContext = dataContext;
      }

      public class MetaCensus
      {
         public string TableName { get; set; }
         public string CensusDesc { get; set; }
         public string GeoType { get; set; }

      }

      // GET: api/map/tableNames
      [HttpGet("tableNames")]
      public ActionResult<List<MetaCensus>> ReadableCensusNames()
      {
         try
         {
            Dictionary<string, string> censusNameDesc = new Dictionary<string, string>();
            List<MetaCensus> metaCensusList = new List<MetaCensus>();
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
               var temp = new MetaCensus();
               temp.GeoType = data.GeoType;
               if (censusNameDesc.Keys.Contains(data.TableName))
               {

                  temp.TableName = data.TableName;
                  temp.CensusDesc = (censusNameDesc[data.TableName]);
                  metaCensusList.Add(temp);
               }
               else
               {
                  temp.TableName = data.TableName;
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
      [HttpGet("heatmap/{tableName}")]
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
                    Error = "Table is not found or is unmappable.",
                    Message = e.ToString()
                }); ;
         }
      }

      // get feature collection for markers
      [HttpGet("markers/{tableName}")]
      public ActionResult<FeatureCollection> GetMarkers(string tableName)
      {
         try
         {
            MapLocationRetriever retriever = new MapLocationRetriever();
            FeatureCollection collection =
                retriever.GetPointFeatureCollection(tableName, _context, _dataContext);
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
