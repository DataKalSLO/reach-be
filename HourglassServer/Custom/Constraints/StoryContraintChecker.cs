using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HourglassServer.Custom.Exceptions;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Custom.Constraints
{
    public enum StoryConstraint
    {
        HAS_USER_ACCOUNT,
        HAS_ADMIN_ACCOUNT,
        AUTHORIZED_USER_OWNS_STORY,
        HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
        HAS_DRAFT_STATUS,
        HAS_PERMISSION_TO_CHANGE_STATUS,
        STORY_EXISTS_WITH_ID
    }

    public class StoryContraintChecker : ConstraintChecker<StoryConstraint, ConstraintEnvironment, StoryApplicationModel>
    {

        public StoryContraintChecker(ConstraintEnvironment env, StoryApplicationModel story) : base(env, story) { }

        override
        protected void CreatePermissions()
        {
            Dictionary<StoryConstraint, Constraint> permissions = new Dictionary<StoryConstraint, Constraint>();
            Dictionary<StoryConstraint, (string message, string tag)> permissionErrors = new Dictionary<StoryConstraint, (string message, string tag)>();

            permissions.Add(StoryConstraint.HAS_USER_ACCOUNT,
                (env, newStory) => env.context.Person.Any(p => p.Email == env.user.GetUserId()));
            permissionErrors.Add(StoryConstraint.HAS_USER_ACCOUNT, (
                "Account required for action.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.HAS_ADMIN_ACCOUNT,
                (env, newStory) => env.user.HasRole(Role.Admin));
            permissionErrors.Add(StoryConstraint.HAS_ADMIN_ACCOUNT,
                ("Administrator account required for action.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.AUTHORIZED_USER_OWNS_STORY,
                HasOwnershipOfStory);
            permissionErrors.Add(StoryConstraint.AUTHORIZED_USER_OWNS_STORY,
                ("Authorized user is not owner of story", ErrorTag.NotOwner));

            permissions.Add(StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                (env, newStory) => this.SatisfiesAtLeastOneConstraint(
                    new StoryConstraint[] { StoryConstraint.AUTHORIZED_USER_OWNS_STORY, StoryConstraint.HAS_ADMIN_ACCOUNT}));
            permissionErrors.Add(StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                ("Authorized user is not owner of story or an administrator.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.HAS_DRAFT_STATUS,
                (env, newStory) => newStory.PublicationStatus == PublicationStatus.DRAFT);
            permissionErrors.Add(StoryConstraint.HAS_DRAFT_STATUS,
                ("Action requires Story to be in DRAFT status.", ErrorTag.BadValue));

            permissions.Add(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS,
                HasPermissionToChangeStatus);
            permissionErrors.Add(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS,
                ("User not permitted to update the status of this story.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.STORY_EXISTS_WITH_ID,
                (env, newStory) => env.context.Story.Any(story => story.StoryId == newStory.Id));
            permissionErrors.Add(StoryConstraint.STORY_EXISTS_WITH_ID,
                ("Could not find an instance of given story.", ErrorTag.NotFound));

            this.constraints = permissions;
            this.constraintErrors = permissionErrors;
        }

        private bool HasOwnershipOfStory(ConstraintEnvironment env, StoryApplicationModel newStory)
        {
            AssertConstraint(StoryConstraint.HAS_USER_ACCOUNT);
            return env.context.Story.Where(story => story.StoryId == newStory.Id).Select(story => story.UserId).Single() == env.user.GetUserId();
        }

        private bool HasPermissionToChangeStatus(ConstraintEnvironment env, StoryApplicationModel newStory)
        {
            Story currentStoryPublicationStatus = env.context.Story
                .AsNoTracking()
                .Where(story => story.StoryId == newStory.Id)
                .Single();
            PublicationStatus oldStatus = StoryFactory.GetPublicationStatus(currentStoryPublicationStatus);
            PublicationStatus newStatus = newStory.PublicationStatus;

            if (newStatus == PublicationStatus.PUBLISHED) // moving to published story
                return this.SatisfiesConstraint(StoryConstraint.HAS_ADMIN_ACCOUNT);
            else
                return true;
        }
    }
}
