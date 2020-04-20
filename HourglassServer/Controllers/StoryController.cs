using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;

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
        public IList<StoryApplicationModel> GetAllStories()
        {
            return StoryModelRetriever.GetAllStoryApplicationModels(_context);
        }

        [HttpGet("{StoryId}")]
        public StoryApplicationModel GetStoryWithId(string StoryId)
        {
            return StoryModelRetriever.GetStoryModelByID(_context, StoryId);
        }

        [HttpPost]
        public string Post([FromBody] StoryApplicationModel story)
        {
            throw new Exception("Method not yet implemented");
        }

        [HttpPut]
        public string Put()
        {
            throw new Exception("Method not yet implemented");
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            throw new Exception("Method not yet implemented");
        }
    }
}
