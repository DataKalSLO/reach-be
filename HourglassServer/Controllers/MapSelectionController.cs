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
    public class MapSelectionController : Controller
    {
        private HourglassContext _context;

        public MapSelectionController(HourglassContext context)
        {
            _context = context;
        }

        // GET: api/mapselection
        // gets all available datasets returned as an array of Strings
        [HttpGet]
        public async Task<List<string>> GetZipCodes()
        {
            try
            {
                List<string> names = new List<string>();
                IEnumerable<DatasetMetaData> metadata = await _context.DatasetMetaData.ToListAsync();
                foreach (DatasetMetaData data in metadata)
                {
                    names.Add(data.TableName);
                }
                return names;
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
