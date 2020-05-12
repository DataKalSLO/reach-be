using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {
        private DatasetDbContext _context;
        public GraphController(DatasetDbContext context)
        {
            //set DatabaseContext in DataSetsController constructor
            _context = context;
        }

        [HttpGet("{category}")]
        public ActionResult<List<storedGraph>> getDefaultGraphs(string category){
            return _context.getDefultGraphs(category).Result;
        }
        // GET api/<controller>/5
        [HttpGet]
        public string Get()
        {
            return "Retrieving graphs not yet implemented";
        }

        // POST api/<controller>
        [HttpPost]
        public string Post()
        {
            return "Creating Graphs is not yet implemented";
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating Graphs is not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete()
        {
            return "Deleting Graphs is not yet implemented";
        }
    }
}
