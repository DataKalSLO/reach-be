namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HourglassServer.Custom.StoryModel;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;
    using HourglassServer.Models.Persistent;
    using HourglassServer.Custom.Exceptions;
    using Microsoft.AspNetCore.Mvc;

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
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.badValue));
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public async Task<IActionResult> GetStoryById(string storyId)
        {
            try
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(HttpContext.User, this.context, new Story { StoryId = storyId });
                permissionChecker.AssertPermission(StoryConstraint.STORY_EXISTS_WITH_ID);
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.badValue));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(HttpContext.User, this.context, storyFromBody);
                permissionChecker.AssertPermission(StoryConstraint.HAS_USER_ACCOUNT);
                storyFromBody.UserId = HttpContext.User.GetUserId();
                IActionResult response = this.context.Story.Any(story => story.StoryId == storyFromBody.Id) ?
                    PerformStoryUpdate(storyFromBody, permissionChecker) :
                    PerformStoryCreation(storyFromBody, permissionChecker);
                await this.context.SaveChangesAsync();
                return response;
            }
            catch(PermissionDeniedException e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), e.errorObj.tag));
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.badValue));
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStoryById(string storyId)
        {
            try
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(HttpContext.User, this.context, new Story() { StoryId = storyId });
                permissionChecker.AssertAllPermissions(new StoryConstraint[]
                {
                    StoryConstraint.STORY_EXISTS_WITH_ID,
                    StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT
                });
                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), ErrorTag.badValue));
            }
        }

        /*
         * Private Helper Methods
         */

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody, StoryConstraintChecker permissionChecker)
        {
            permissionChecker.AssertPermission(StoryConstraint.HAS_DRAFT_STATUS);
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        private IActionResult PerformStoryUpdate(StoryApplicationModel storyFromBody, StoryConstraintChecker permissionChecker)
        {
            permissionChecker.AssertPermission(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS);
            StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
            return new NoContentResult();
        }
    }
}
