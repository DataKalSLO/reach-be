namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using HourglassServer.Custom.StoryModel;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;
    using HourglassServer.Models.Persistent;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    // TODO: Catch different types of exceptions and return descriptive tags for all routes.
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private const string badValueTag = "badValue";
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
                return this.BadRequest(new HourglassError(e.ToString(), badValueTag));
            }
        }

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public async Task<IActionResult> GetStoryById(string storyId)
        {
            try
            {
                StoryPermissionCheckers permissionChecker = new StoryPermissionCheckers(HttpContext.User, this.context, new Story { StoryId = storyId });
                permissionChecker.AssertPermission(StoryActionConstraint.STORY_EXISTS_WITH_ID);
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), badValueTag));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                StoryPermissionCheckers permissionChecker = new StoryPermissionCheckers(HttpContext.User, this.context, storyFromBody);
                permissionChecker.AssertPermission(StoryActionConstraint.HAS_USER_ACCOUNT);
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
                return this.BadRequest(new HourglassError(e.ToString(), badValueTag));
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStoryById(string storyId)
        {
            try
            {
                StoryPermissionCheckers permissionChecker = new StoryPermissionCheckers(HttpContext.User, this.context, new Story() { StoryId = storyId });
                permissionChecker.AssertAllPermissions(new StoryActionConstraint[]
                {
                    StoryActionConstraint.STORY_EXISTS_WITH_ID,
                    StoryActionConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT
                });
                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassError(e.ToString(), "badValue"));
            }
        }

        /*
         * Private Helper Methods
         */

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody, StoryPermissionCheckers permissionChecker)
        {
            permissionChecker.AssertAllPermissions(new StoryActionConstraint[]
            {
                StoryActionConstraint.STORY_USER_MATCHES_AUTHORIZED_USER,
                StoryActionConstraint.HAS_DRAFT_STATUS  
            });
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        private IActionResult PerformStoryUpdate(StoryApplicationModel storyFromBody, StoryPermissionCheckers permissionChecker)
        {
            permissionChecker.AssertAtLeastOnePermission(new StoryActionConstraint[]
            {
                StoryActionConstraint.STORY_USER_MATCHES_AUTHORIZED_USER,
                StoryActionConstraint.HAS_ADMIN_ACCOUNT
            });
            permissionChecker.AssertPermission(StoryActionConstraint.HAS_PERMISSION_TO_CHANGE_STATUS);
            StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
            return new NoContentResult();
        }
    }
}
