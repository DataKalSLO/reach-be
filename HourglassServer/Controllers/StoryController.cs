using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.Persistent;

using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private postgresContext _context;

        public StoryController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/Story
        [HttpGet]
        public IList<StoryCreationObject> Get()
        {
            throw new Exception("Method not implemented yet.");
        }

        // GET: api/Story/UUID
        [HttpGet("{id}")]
        public StoryCreationObject Get(string id)
        {
            throw new Exception("Method not implemented yet.");
        }

        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody] StoryCreationObject story)
        {
            throw new Exception("Method not implemented yet.");
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating stories not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            throw new Exception("Method not implemented yet.");
        }

        [HttpGet("count")]
        public string getNumberOfStories()
        {
            return _context.Story.Count().ToString();
        }
    }
}
