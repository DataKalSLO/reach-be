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
    public class StoryContraintChecker: ConstraintChecker<StoryApplicationModel>
    {

        public StoryContraintChecker(ConstraintEnvironment env, StoryApplicationModel story) : base(env, story) { }

        protected override void CreatePermissions()
        {
            base.CreatePermissions();
            
            Constraints.Add(Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY,
                HasOwnershipOfStory);
            ConstraintErrors.Add(Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY,
                ("Authorized user is not owner of story", ErrorTag.NotOwner));

            Constraints.Add(Custom.Constraints.Constraints.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                (env, newStory) => this.SatisfiesAtLeastOneConstraint(
                    new Constraints[] { Custom.Constraints.Constraints.AUTHORIZED_USER_OWNS_STORY, Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT}));
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                ("Authorized user is not owner of story or an administrator.", ErrorTag.ForbiddenRole));

            Constraints.Add(Custom.Constraints.Constraints.HAS_DRAFT_STATUS,
                (env, newStory) => newStory.PublicationStatus == PublicationStatus.DRAFT);
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_DRAFT_STATUS,
                ("Action requires Story to be in DRAFT status.", ErrorTag.BadValue));

            Constraints.Add(Custom.Constraints.Constraints.HAS_PERMISSION_TO_CHANGE_STATUS,
                HasPermissionToChangeStatus);
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_PERMISSION_TO_CHANGE_STATUS,
                ("User not permitted to update the status of this story.", ErrorTag.ForbiddenRole));

            Constraints.Add(Custom.Constraints.Constraints.STORY_EXISTS_WITH_ID,
                (env, newStory) => env.context.Story.Any(story => story.StoryId == newStory.Id));
            ConstraintErrors.Add(Custom.Constraints.Constraints.STORY_EXISTS_WITH_ID,
                ("Could not find an instance of given story.", ErrorTag.NotFound));
        }

        private bool HasOwnershipOfStory(ConstraintEnvironment env, StoryApplicationModel newStory)
        {
            AssertConstraint(Custom.Constraints.Constraints.HAS_USER_ACCOUNT);
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
                return this.SatisfiesConstraint(Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT);
            else
                return true;
        }
    }
}
