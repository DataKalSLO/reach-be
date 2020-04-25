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
        public StoryApplicationModel GetStoryById(string storyId)
        {
            return StoryModelRetriever.GetStoryApplicationModelById(_context, storyId);
        }

        [HttpPost]
        public IActionResult CreateStory([FromBody] StoryApplicationModel story)
        {
            try
            {
                StoryApplicationModel storyCreated = StoryModelCreator.AddStoryApplicationModelToDatabaseContext(_context, story);
                _context.SaveChanges();
                return new OkObjectResult(storyCreated.Id);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPut]
        public IActionResult UpdateStory(StoryApplicationModel story)
        {
            try
            {
                StoryApplicationModel storyCreated = StoryModelUpdater.UpdateStoryApplicationModel(_context, story);
                _context.SaveChanges();
                return new OkObjectResult(storyCreated.Id);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStoryById(string storyId)
        {
            try
            {
                StoryModelDeleter.DeleteStoryByID(_context, storyId);
                _context.SaveChanges();
                return new OkObjectResult("success");
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }
    }
}
