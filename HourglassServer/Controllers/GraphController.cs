using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {

        // GET api/<controller>/5
        [HttpGet("{id}")]
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
