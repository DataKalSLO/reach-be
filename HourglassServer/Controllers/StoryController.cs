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

    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private readonly HourglassContext context;

        public StoryController(HourglassContext context)
        {
            this.context = context;
        }

        /*
         * The following methods specify the endpoints for api/story
         * These methods are responsible to asserting constraints/permissions.
         */

        [HttpGet]
        public IActionResult GetAllPublishedStories()
        {
            return HandleGetStoriesInPublicationStatus(PublicationStatus.PUBLISHED);
        }

        [HttpGet("review")]
        public IActionResult GetStoriesInReviewForUser()
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
        public IActionResult GetStoriesInDraftForUser()
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

        [HttpGet("{storyId}", Name = nameof(GetStoryById))]
        public async Task<IActionResult> GetStoryById(string storyId)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                    new ConstraintEnvironment(this.HttpContext.User, context),
                    new StoryApplicationModel { Id = storyId });

                permissionChecker.AssertConstraint(Constraints.STORY_EXISTS_WITH_ID);

                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(this.context, storyId);
                await this.context.SaveChangesAsync();
                return new OkObjectResult(storyWithId);
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
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
         * Story feedback CRUD operations
         */

        //Creators

        [HttpPost("feedback")]
        public async Task<IActionResult> CreateStoryFeedback([FromBody] StoryFeedback feedback)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                StoryConstraintChecker permissionChecker = new StoryConstraintChecker(
                    new ConstraintEnvironment(HttpContext.User, context), null);
                permissionChecker.AssertConstraint(Constraints.HAS_ADMIN_ACCOUNT);

                feedback.ReviewerId = HttpContext.User.GetUserId();

                bool storyFeedbackExists = context.StoryFeedback.Any(
                    storyFeedback => storyFeedback.FeedbackId == feedback.FeedbackId);
                IActionResult result;
                if (storyFeedbackExists)
                {
                    context.Update(feedback);
                    result = new NoContentResult();
                }
                else
                {
                    feedback.FeedbackId = System.Guid.NewGuid().ToString();
                    context.StoryFeedback.Add(feedback);
                    result = new CreatedAtRouteResult(new { feedbackId = feedback.FeedbackId }, feedback);
                }
                await context.SaveChangesAsync();
                return result;
            });
        }

        //Retrievers

        [HttpGet("feedback/{storyId}")]
        public IActionResult GetStoryFeedbackByStoryId(string storyId)
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                IList<StoryFeedback> feedbacks = context.StoryFeedback
                    .Where(storyFeedback => storyFeedback.StoryId == storyId)
                    .ToList();
                return new OkObjectResult(feedbacks);
            });
        }

        //Updates - Merged in CreateStoryFeedback.

        //Deletors

        [HttpDelete("feedback/{feedbackId}")]
        public async Task<IActionResult> DeleteStoryFeedbackById(string feedbackId)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                StoryFeedback feedback = context.StoryFeedback
                    .Find(feedbackId);
                await context.DeleteAsync(feedback);
                return new OkObjectResult(feedback);
            });
        }
        /*
         * Private Helper Methods
         */

        private IActionResult HandleGetStoriesInPublicationStatus(PublicationStatus expectedStatus)
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                IList<StoryApplicationModel> allStories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(this.context, expectedStatus);
                return new OkObjectResult(allStories);
            });
        }

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody, StoryConstraintChecker permissionChecker)
        {
            permissionChecker.AssertConstraint(Constraints.HAS_DRAFT_STATUS);
            storyFromBody.UserId = HttpContext.User.GetUserId();  //Asserts that authenticated user is the owner
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        private IActionResult PerformStoryUpdate(StoryApplicationModel storyFromBody, StoryConstraintChecker permissionChecker)
        {
            permissionChecker.AssertConstraint(Constraints.HAS_PERMISSION_TO_CHANGE_STATUS);
            storyFromBody.UserId = context.Story.
                Where(story => story.StoryId == storyFromBody.Id)
                .Select(story => story.UserId)
                .Single(); //Fills potentially null value with real owner
            StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
            return new NoContentResult();
        }
    }
}
