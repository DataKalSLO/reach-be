namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HourglassServer.Custom.Constraints;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;
    using HourglassServer.Custom.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    // TODO: Catch different types of exceptions and return descriptive tags for all routes.
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
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
                return new OkObjectResult(allStories);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.BadValue));
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public async Task<IActionResult> GetStoryById(string storyId)
        {
            try
            {
                StoryContraintChecker permissionChecker = new StoryContraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context),
                    new StoryApplicationModel { Id = storyId });

                permissionChecker.AssertConstraint(StoryConstraint.STORY_EXISTS_WITH_ID);

                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }
            catch(HourglassError e)
            {
                return this.BadRequest(e);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.BadValue));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                StoryContraintChecker permissionChecker = new StoryContraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context),
                    storyFromBody);

                permissionChecker.AssertConstraint(StoryConstraint.HAS_USER_ACCOUNT);

                IActionResult response = this.context.Story.Any(story => story.StoryId == storyFromBody.Id) ?
                    PerformStoryUpdate(storyFromBody, permissionChecker) :
                    PerformStoryCreation(storyFromBody, permissionChecker);
                await this.context.SaveChangesAsync();
                return response;
            }
            catch (HourglassError e)
            {
                return this.BadRequest(e);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.BadValue));
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStoryById(string storyId)
        {
            try
            {
                StoryContraintChecker permissionChecker = new StoryContraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context),
                    new StoryApplicationModel() { Id = storyId });

                permissionChecker.AssertAllConstraints(new StoryConstraint[]
                {
                    StoryConstraint.STORY_EXISTS_WITH_ID, //only checks the id set above
                    StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT
                });

                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (HourglassError e)
            {
                return this.BadRequest(e);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.BadValue));
            }
        }

        /*
         * Private Helper Methods
         */

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody, StoryContraintChecker permissionChecker)
        {
            permissionChecker.AssertConstraint(StoryConstraint.HAS_DRAFT_STATUS);
            storyFromBody.UserId = HttpContext.User.GetUserId();  //Asserts that authenticated user is the owner
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        private IActionResult PerformStoryUpdate(StoryApplicationModel storyFromBody, StoryContraintChecker permissionChecker)
        {
            permissionChecker.AssertConstraint(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS);
            storyFromBody.UserId = context.Story.
                Where(story => story.StoryId == storyFromBody.Id)
                .Select(story => story.UserId)
                .Single(); //Fills potentially null value with real owner
            StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
            return new NoContentResult();
        }
    }
}
