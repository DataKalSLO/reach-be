using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HourglassServer.Custom.Exception;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryOperations;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Custom.Constraints
{
    public class StoryConstraintChecker: ConstraintChecker<StoryApplicationModel>
    {

        public StoryConstraintChecker(ConstraintEnvironment env, StoryApplicationModel story) : base(env, story) { }

        protected override void CreatePermissions()
        {
            base.CreatePermissions();
            
            Constraints.Add(Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY,
                HasOwnershipOfStory);
            ConstraintErrors.Add(Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY,
                ("Authorized user is not owner of story", ExceptionTag.NotOwner));

            Constraints.Add(Custom.Constraints.Constraints.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                (env, newStory) => this.SatisfiesAtLeastOneConstraint(
                    new Constraints[] { Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY, Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT}));
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                ("Authorized user is not owner of story or an administrator.", ExceptionTag.ForbiddenRole));

            Constraints.Add(Custom.Constraints.Constraints.HAS_DRAFT_STATUS,
                (env, newStory) => newStory.PublicationStatus == PublicationStatus.DRAFT);
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_DRAFT_STATUS,
                ("Action requires Story to be in DRAFT status.", ExceptionTag.BadValue));

            Constraints.Add(Custom.Constraints.Constraints.HAS_PERMISSION_TO_CHANGE_STATUS,
                HasPermissionToChangeStatus);
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_PERMISSION_TO_CHANGE_STATUS,
                ("User not permitted to update the status of this story.", ExceptionTag.ForbiddenRole));

            Constraints.Add(Custom.Constraints.Constraints.STORY_EXISTS_WITH_ID,
                (env, newStory) => env.context.Story.Any(story => story.StoryId == newStory.Id));
            ConstraintErrors.Add(Custom.Constraints.Constraints.STORY_EXISTS_WITH_ID,
                ("Could not find an instance of given story.", ExceptionTag.NotFound));
        }

        private bool HasOwnershipOfStory(ConstraintEnvironment env, StoryApplicationModel newStory)
        {
            AssertConstraint(Custom.Constraints.Constraints.HAS_USER_ACCOUNT);
            return env.context.Story.Where(story => story.StoryId == newStory.Id).Select(story => story.UserId).Single() == env.user.GetUserId();
        }

        private bool HasPermissionToChangeStatus(ConstraintEnvironment env, StoryApplicationModel newStory)
        {
            if (newStory.PublicationStatus == PublicationStatus.PUBLISHED ||
                newStory.PublicationStatus == PublicationStatus.FEEDBACK) // moving beyond review
                return this.SatisfiesConstraint(Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT);
            else
                return true;
        }
    }
}
