using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Net;

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
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetAllStoryApplicationModels(_context);
                _context.SaveChanges();
                return new OkObjectResult(allStories);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
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
                IActionResult response; 
                bool storyExists = _context.Story.Any(story => story.StoryId == storyFromBody.Id);
                if (storyExists)
                {
                    StoryModelUpdater.UpdateStoryApplicationModel(_context, storyFromBody);
                    response = new NoContentResult();
                }
                else
                {
                    StoryModelCreator.AddStoryApplicationModelToDatabaseContext(_context, storyFromBody);
                    response = new CreatedAtRouteResult(nameof(GetStoryById),new { storyId = storyFromBody.Id }, storyFromBody) ;
                }
                _context.SaveChanges();
                return response;
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpDelete("{storyId}")]
        public IActionResult DeleteStoryById(string storyId)
        {
            try
            {
                StoryModelDeleter.DeleteStoryById(_context, storyId);
                _context.SaveChanges();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }
    }
}
