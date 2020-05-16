namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using HourglassServer.Data;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.StoryModel;
    using HourglassServer.Models.Persistent;
    using Microsoft.AspNetCore.Mvc;

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
                return this.BadRequest(new[] { new HourglassError(e.ToString(), badValueTag) });
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
                return this.BadRequest(new[] { new HourglassError(e.ToString(), badValueTag) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ModifyStory([FromBody] StoryApplicationModel storyFromBody)
        {
            try
            {
                string userId = Utilities.GetUserId(HttpContext.User);
                StoryPermissionCheckers.AssertPermission(HttpContext.User, StoryActionConstraint.ACCOUNT_REQUIRED, storyFromBody);
                IActionResult response = this.context.Story.Any(story => story.StoryId == storyFromBody.Id) ? PerformStoryUpdate(storyFromBody) : PerformStoryCreation(storyFromBody);
                await this.context.SaveChangesAsync();
                return response;
            }
            catch(PermissionDeniedException e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), e.errorObj.tag) });
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), badValueTag) });
            }
        }

        private IActionResult PerformStoryCreation(StoryApplicationModel storyFromBody)
        {
            StoryPermissionCheckers.AssertAllPermissions(HttpContext.User, new StoryActionConstraint[]
            {
                StoryActionConstraint.STORY_STATUS_DRAFT,
                StoryActionConstraint.STORY_OWNERSHIP
            }, storyFromBody);
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(this.context, storyFromBody);
            return new CreatedAtRouteResult(nameof(this.GetStoryById), new { storyId = storyFromBody.Id }, storyFromBody);
        }

        private IActionResult PerformStoryUpdate(StoryApplicationModel storyFromBody)
        {
            AssertUserHasPermissionToUpdateStory(HttpContext.User, storyFromBody);
            StoryModelUpdater.UpdateStoryApplicationModel(this.context, storyFromBody);
            return new NoContentResult();
        }

        private void AssertUserHasPermissionToUpdateStory(ClaimsPrincipal user, StoryApplicationModel newStory)
        {
            string currentStoryPublicationStatus = this.context.Story
                       .Where(story => story.StoryId == newStory.Id)
                       .Select(story => story.PublicationStatus).Single();
            PublicationStatus oldStatus;
            Enum.TryParse(currentStoryPublicationStatus, true, out oldStatus);
            PublicationStatus newStatus = newStory.PublicationStatus;

            if (oldStatus < newStatus && newStatus == PublicationStatus.PUBLISHED) // moving to published story
                StoryPermissionCheckers.AssertPermission(user, StoryActionConstraint.ADMIN_USER_REQUIRED, newStory);
            else if (oldStatus > newStatus && oldStatus == PublicationStatus.PUBLISHED) // reverting published story
                StoryPermissionCheckers.AssertPermission(user, StoryActionConstraint.ADMIN_USER_REQUIRED, newStory);
            else // moving draft->review (or vica versa) || no status change
                StoryPermissionCheckers.AssertAtLeastOnePermission(user, new StoryActionConstraint[]
                    {
                        StoryActionConstraint.STORY_OWNERSHIP,
                        StoryActionConstraint.ADMIN_USER_REQUIRED
                    }, newStory);
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

    enum StoryActionConstraint
    {
        OPEN,
        ACCOUNT_REQUIRED,
        ADMIN_USER_REQUIRED,
        STORY_OWNERSHIP,
        STORY_STATUS_DRAFT
    }

    class StoryPermissionCheckers
    {
        private const string nowOwnerTag = "notOwner"; //TODO: Generalize this
        private const string forbiddenRoleTag = "forbiddenRole";

        public static void AssertAtLeastOnePermission(ClaimsPrincipal user, StoryActionConstraint[] constraints, StoryApplicationModel storyToModify)
        {
            List<Exception> exceptions = new List<Exception>(); 
            foreach (StoryActionConstraint constraint in constraints)
            {
                try
                {
                    AssertPermission(user, constraint, storyToModify);
                    return; // condition satisfied
                } catch(Exception e)
                {
                    exceptions.Add(e);
                }
            }
            throw exceptions[0];
        }

        public static bool AssertAllPermissions(ClaimsPrincipal user, StoryActionConstraint[] constraints, StoryApplicationModel storyToModify)
        {
            foreach(StoryActionConstraint constraint in constraints)
            {
                if (!SatisfiesPermission(user, constraint, storyToModify))
                    return false;
            }
            return true; 
        }

        public static bool SatisfiesPermission(ClaimsPrincipal user, StoryActionConstraint action, StoryApplicationModel storyToModify)
        {
            try
            {
                AssertPermission(user, action, storyToModify);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public static bool AssertPermission(ClaimsPrincipal user, StoryActionConstraint action, StoryApplicationModel storyToModify)
        {
            //Note, these check are checking for their negative condition because they throw errors.
            switch(action)
            {
                case StoryActionConstraint.OPEN:
                    return true;
                case StoryActionConstraint.ACCOUNT_REQUIRED:
                    if (!user.HasRole(Role.Admin) && !user.HasRole(Role.BaseUser))
                        throw new PermissionDeniedException("Account required for action.", forbiddenRoleTag);
                    return true;
                case StoryActionConstraint.ADMIN_USER_REQUIRED:
                    if (!user.HasRole(Role.Admin))
                        throw new PermissionDeniedException("Administrator account required for action.", forbiddenRoleTag);
                    return true;
                case StoryActionConstraint.STORY_OWNERSHIP:
                    if (storyToModify.UserId != user.GetUserId())
                        throw new PermissionDeniedException("Authorized user is not owner of story", nowOwnerTag);
                    return true;
                case StoryActionConstraint.STORY_STATUS_DRAFT:
                    if (storyToModify.PublicationStatus != PublicationStatus.DRAFT)
                        throw new PermissionDeniedException("Action requires Story to be in DRAFT status.", forbiddenRoleTag);
                    return true;
                default:
                    throw new InvalidOperationException("Could not identify constraint: " + action);

            }
        }
    }
}
