/*
 * The following methods specify the endpoints for api/story
 * These methods are responsible to asserting constraints/permissions.
 */
namespace HourglassServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryOperations;
    using HourglassServer.Custom.Constraints;
    using HourglassServer.Custom.Exception;
    using HourglassServer.Models.Persistent;

    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private readonly HourglassContext context;

        public StoryController(HourglassContext context)
        {
            this.context = context;
        }

        /*
         *  Story CRUD Operations
         */

        //Creators

        [HttpPost]
        public async Task<IActionResult> CreateOrModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            return await runAsyncApiOperation(async () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                   new ConstraintEnvironment(this.HttpContext.User, context),
                   storyFromBody);

                permissionChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);

                IActionResult response = this.context.Story.Any(story => story.StoryId == storyFromBody.Id) ?
                    PerformStoryUpdate(storyFromBody, permissionChecker) :
                    PerformStoryCreation(storyFromBody, permissionChecker);
                await this.context.SaveChangesAsync();
                return response;
            }); 
        }

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody, StoryConstraintChecker permissionChecker)
        {
            permissionChecker.AssertConstraint(Constraints.HAS_DRAFT_STATUS);
            storyFromBody.UserId = HttpContext.User.GetUserId();  //Asserts that authenticated user is the owner
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.RetrieveStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        //Retrievers

        [HttpGet]
        public IActionResult RetrieveAllPublishedStories()
        {
            return HandleGetStoriesInPublicationStatus(PublicationStatus.PUBLISHED);
        }

        public async Task<IActionResult> RetrieveStoryById(string storyId)
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                permissionChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);

                string userId = HttpContext.User.GetUserId();
                if (HttpContext.User.HasRole(Role.Admin))
                {
                    return HandleGetStoriesInPublicationStatus(PublicationStatus.REVIEW);
                }
                else
                {
                    IList<StoryApplicationModel> storiesInReviewForUser =
                        StoryModelRetriever.GetStoryApplicationModelsInPublicationStatusByUserId(
                            this.context,
                            PublicationStatus.REVIEW,
                            userId);
                    return new OkObjectResult(storiesInReviewForUser);
                }
            }); 
        }

        [HttpGet("draft")]
        public IActionResult RetrieveStoriesInDraftForUser()
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                   new ConstraintEnvironment(this.HttpContext.User, context), null);
                permissionChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);

                string userId = HttpContext.User.GetUserId();
                IList<StoryApplicationModel> storiesInReviewForUser =
                    StoryModelRetriever.GetStoryApplicationModelsInPublicationStatusByUserId(
                        this.context,
                        PublicationStatus.DRAFT,
                        userId);
                return new OkObjectResult(storiesInReviewForUser);
            }); 
        }

        [HttpGet("review")]
        public IActionResult RetrieveStoriesInReviewForUser()
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                permissionChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);

                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }); 
        }

        private IActionResult HandleGetStoriesInPublicationStatus(PublicationStatus expectedStatus)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(this.context, expectedStatus);
                return new OkObjectResult(allStories);
            });
        }

        //Updater - Note, Create endpoint is combined with update endpoint.

                IActionResult response = this.context.Story.Any(story => story.StoryId == storyFromBody.Id) ?
                    PerformStoryUpdate(storyFromBody, permissionChecker) :
                    PerformStoryCreation(storyFromBody, permissionChecker);
                await this.context.SaveChangesAsync();
                return response;
            });
        }

        //Deletors

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStoryById(string storyId)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context),
                    new StoryApplicationModel() { Id = storyId });

                permissionChecker.AssertAllConstraints(new Constraints[]
                {
                    Constraints.STORY_EXISTS_WITH_ID, //only checks the id set above
                    Constraints.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT
                });

                StoryModelDeleter.DeleteStoryById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new NoContentResult();
            }); 
        }

        /*
         * Private Helper Methods
         */

        public async Task<IActionResult> runAsyncApiOperation(Func<Task<IActionResult>> action)
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(this.context, expectedStatus);
                return new OkObjectResult(allStories);
            });
        }

        private IActionResult runApiOperation(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (HourglassException e)
            {
                return this.BadRequest(e);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }
    }
}
