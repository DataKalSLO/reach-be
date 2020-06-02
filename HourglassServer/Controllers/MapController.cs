using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class MapController : Controller
    {
        // GET: api/<controller>
        [HttpGet("{id}")]
        public string Get()
        {
            return "Retrieving Maps is not yet implemented.";
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
