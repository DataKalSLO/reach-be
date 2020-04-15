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
        public IList<StoryApplicationModel> Get()
        {
            throw new Exception("Method not yet implemented");
        }

        [HttpGet("{id}")]
        public StoryApplicationModel Get(string id)
        {
            throw new Exception("Method not yet implemented");
        }

        [HttpPost]
        public StoryApplicationModel Post([FromBody] StoryApplicationModel story)
        {
            throw new Exception("Method not yet implemented");
        }

        [HttpPut]
        public string Put()
        {
            return "Updating stories not yet implemented";
        }

        [HttpDelete("{id}")]
        public StoryApplicationModel Delete(string id)
        {
            throw new Exception("Method not yet implemented");
        }
    }
}
