using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private const string successMessage = "success";
        private readonly HourglassContext _context;

        public StoryController(HourglassContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStories()
        {
            try
            {
                IList<StoryApplicationModel> storyWithId = StoryModelRetriever.GetAllStoryApplicationModels(_context);
                _context.SaveChanges();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpGet("{StoryId}")]
        public IActionResult GetStoryById(string storyId)
        {
            try
            {
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(_context, storyId);
                _context.SaveChanges();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPost]
        public IActionResult ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                bool storyExists = _context.Story.Any(story => story.StoryId == storyFromBody.Id);
                if (storyExists)
                   StoryModelUpdater.UpdateStoryApplicationModel(_context, storyFromBody);
                else
                    StoryModelCreator.AddStoryApplicationModelToDatabaseContext(_context, storyFromBody);
                _context.SaveChanges();
                return new OkObjectResult(successMessage);
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
                StoryModelDeleter.DeleteStoryById(_context, storyId);
                _context.SaveChanges();
                return new OkObjectResult(successMessage);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }
    }
}
