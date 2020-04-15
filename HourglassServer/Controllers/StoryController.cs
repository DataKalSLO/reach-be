using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private HourglassContext _context;

        public StoryController(HourglassContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IList<StoryCreationObject> Get()
        {
            throw new Exception("Method not implemented yet.");
        }

        [HttpGet("{id}")]
        public StoryCreationObject Get(string id)
        {
            throw new Exception("Method not implemented yet.");
        }

        [HttpPost]
        public string Post([FromBody] StoryCreationObject story)
        {
            throw new Exception("Method not implemented yet.");
        }

        [HttpPut]
        public string Put()
        {
            return "Updating stories not yet implemented";
        }

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
