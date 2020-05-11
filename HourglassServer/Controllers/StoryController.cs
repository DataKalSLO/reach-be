namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;
    using HourglassServer.Models.Persistent;
    using Microsoft.AspNetCore.Mvc;

    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private const string SuccessMessage = "success";
        private readonly HourglassContext context;

        public StoryController(HourglassContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAllStories()
        {
            try
            {
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetAllStoryApplicationModels(this.context);
                this.context.SaveChanges();
                return new OkObjectResult(allStories);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public IActionResult GetStoryById(string storyId)
        {
            try
            {
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                this.context.SaveChanges();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPost]
        public IActionResult ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                IActionResult response; 
                bool storyExists = this.context.Story.Any(story => story.StoryId == storyFromBody.Id);
                if (storyExists)
                {
                    StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
                    response = new NoContentResult();
                }
                else
                {
                    StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
                    response = new CreatedAtRouteResult(nameof(this.GetStoryById),new { storyId = storyFromBody.Id }, storyFromBody) ;
                }

                this.context.SaveChanges();
                return response;
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpDelete("{storyId}")]
        public IActionResult DeleteStoryById(string storyId)
        {
            try
            {
                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                this.context.SaveChanges();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }
    }
}
