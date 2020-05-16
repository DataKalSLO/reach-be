namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;

    // TODO: Catch different types of exceptions and return descriptive tags for all routes.
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private const string MissingAdminPrivileges = "Admin privileges are required to access this action.";
        private readonly HourglassContext context;

        public StoryController(HourglassContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetStoriesInPublished()
        {
            return HandleGetStoriesInPublicationStatus(PublicationStatus.PUBLISHED);
        }

        [HttpGet("review")]
        public IActionResult GetStoriesInReview()
        {
            if (!HttpContext.User.HasRole(Role.Admin))
                return BadRequest(new HourglassError(MissingAdminPrivileges, "forbiddenRole"));
            return HandleGetStoriesInPublicationStatus(PublicationStatus.REVIEW);
        }

        [HttpGet("draft")]
        public IActionResult GetStoriesInDraftForUser()
        {
            try
            {
                string userId = Utilities.GetUserIdFromToken(this.HttpContext.User.Claims);
                IList<StoryApplicationModel> storiesInReviewForUser = StoryModelRetriever.GetStoryApplicationModelsInDraft(this.context, userId);
                return new OkObjectResult(storiesInReviewForUser);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        private IActionResult HandleGetStoriesInPublicationStatus(PublicationStatus expectedStatus)
        {
            try
            {
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(this.context, expectedStatus);
                return new OkObjectResult(allStories);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public async Task<IActionResult> GetStoryById(string storyId)
        {
            try
            {
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
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
                    response = new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
                }

                await this.context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStoryById(string storyId)
        {
            try
            {
                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }
    }
}
