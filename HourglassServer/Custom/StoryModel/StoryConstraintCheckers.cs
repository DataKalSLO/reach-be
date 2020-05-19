using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using HourglassServer.Custom.Exceptions;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Custom.StoryModel
{
    enum StoryConstraint
    {
        HAS_USER_ACCOUNT,
        HAS_ADMIN_ACCOUNT,
        AUTHORIZED_USER_OWNS_STORY,
        HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
        HAS_DRAFT_STATUS,
        HAS_PERMISSION_TO_CHANGE_STATUS,
        STORY_EXISTS_WITH_ID
    }

    class StoryConstraintChecker
    {
        public delegate bool permission(ClaimsPrincipal user, HourglassContext context, Story newStory);

        private Dictionary<StoryConstraint, permission> permissions = new Dictionary<StoryConstraint, permission>();
        private Dictionary<StoryConstraint, (string message, string tag)> permissionErrors = new Dictionary<StoryConstraint, (string message, string tag)>();
        private ClaimsPrincipal user;
        private Story newStory;
        private HourglassContext context;

        public StoryConstraintChecker(ClaimsPrincipal user, HourglassContext context)
        {
            this.user = user;
            this.context = context;
            this.newStory = null;
            CreatePermissions();
        }

        public StoryConstraintChecker(ClaimsPrincipal user, HourglassContext context, StoryApplicationModel newStory) : this(user, context)
        {
            this.newStory = StoryFactory.CreateStoryFromStoryModel(newStory);
        }

        public StoryConstraintChecker(ClaimsPrincipal user, HourglassContext context, Story newStory) : this(user, context)
        {
            this.newStory = newStory;
        }

        private void CreatePermissions()
        {
            permissions.Add(StoryConstraint.HAS_USER_ACCOUNT, (user, context, newStory) => context.Person.Any(p => p.Email == user.GetUserId()));
            permissionErrors.Add(StoryConstraint.HAS_USER_ACCOUNT, ("Account required for action.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.HAS_ADMIN_ACCOUNT, (user, context, newStory) => user.HasRole(Role.Admin));
            permissionErrors.Add(StoryConstraint.HAS_ADMIN_ACCOUNT, ("Administrator account required for action.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.AUTHORIZED_USER_OWNS_STORY, HasOwnershipOfStory);
            permissionErrors.Add(StoryConstraint.AUTHORIZED_USER_OWNS_STORY, ("Authorized user is not owner of story", ErrorTag.NotOwner));

            permissions.Add(StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                (user, context, newStory) => SatisfiesAtLeastOnePermission(new StoryConstraint[] { StoryConstraint.AUTHORIZED_USER_OWNS_STORY, StoryConstraint.HAS_ADMIN_ACCOUNT }));
            permissionErrors.Add(StoryConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT, ("Authorized user is not owner of story or an administrator.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.HAS_DRAFT_STATUS, (user, context, newStory) => StoryFactory.GetPublicationStatus(newStory) == PublicationStatus.DRAFT);
            permissionErrors.Add(StoryConstraint.HAS_DRAFT_STATUS, ("Action requires Story to be in DRAFT status.", ErrorTag.BadValue));

            permissions.Add(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS, HasPermissionToChangeStatus);
            permissionErrors.Add(StoryConstraint.HAS_PERMISSION_TO_CHANGE_STATUS, ("User not permitted to update the status of this story.", ErrorTag.ForbiddenRole));

            permissions.Add(StoryConstraint.STORY_EXISTS_WITH_ID, (user, context, newStory) => context.Story.Any(story => story.StoryId == newStory.StoryId));
            permissionErrors.Add(StoryConstraint.STORY_EXISTS_WITH_ID, ("Could not find an instance of given story.", ErrorTag.QueryFailed));
        }

        public void AssertAtLeastOnePermission(StoryConstraint[] constraints)
        {
            if (!SatisfiesAtLeastOnePermission(constraints))
                ThrowPermissionException(constraints[0]);
        }

        public void AssertAllPermissions(StoryConstraint[] constraints)
        {
            (bool result, StoryConstraint? potentialFailingAction) = SatisfiesAllPermissions(constraints);
            if (potentialFailingAction is StoryConstraint failingAction)
                ThrowPermissionException(failingAction);
        }

        public void AssertPermission(StoryConstraint action)
        {
            if (!this.SatisfiesPermission(action))
                ThrowPermissionException(action);
        }

        public void AssertPermission(permission action, string message, string tag)
        {
            if (!this.SatisfiesPermission(action))
                ThrowPermissionException(message, tag);
        }

        public void ThrowPermissionException(StoryConstraint action)
        {
            throw new PermissionDeniedException(permissionErrors[action].message, permissionErrors[action].tag);
        }

        public void ThrowPermissionException(string message, string tag)
        {
            throw new PermissionDeniedException(message, tag);
        }

        /*
         * Satisfies
         */

        public bool SatisfiesAtLeastOnePermission(StoryConstraint[] constraints)
        {
            if (constraints.Count() == 0)
                throw new InvalidOperationException("Expected at least one constraint.");
            foreach (StoryConstraint constraint in constraints)
            {
                if (this.SatisfiesPermission(constraint))
                    return true;
            }
            return false;
        }

        public (bool result, StoryConstraint? fail) SatisfiesAllPermissions(StoryConstraint[] constraints)
        {
            if (constraints.Count() == 0)
                throw new InvalidOperationException("Expected at least one constraint.");
            foreach (StoryConstraint constraint in constraints)
            {
                if (!this.SatisfiesPermission(constraint))
                    return (false, constraint);
            }
            return (true, null);
        }

        public bool SatisfiesPermission(StoryConstraint action)
        {
            return permissions[action](this.user, this.context, this.newStory);
        }

        public bool SatisfiesPermission(permission action)
        {
            return action(this.user, this.context, this.newStory);
        }

        /*
         * Custom Permissions
         */

        private bool HasOwnershipOfStory(ClaimsPrincipal user, HourglassContext context, Story newStory)
        {
            AssertPermission(StoryConstraint.HAS_USER_ACCOUNT);
            return context.Story.Where(story => story.StoryId == newStory.StoryId).Select(story => story.UserId).Single() == user.GetUserId();
        }

        private bool HasPermissionToChangeStatus(ClaimsPrincipal user, HourglassContext context, Story newStory)
        {
            Story currentStoryPublicationStatus = context.Story
                .AsNoTracking()
                .Where(story => story.StoryId == newStory.StoryId)
                .Single();
            PublicationStatus oldStatus = StoryFactory.GetPublicationStatus(currentStoryPublicationStatus);
            PublicationStatus newStatus = StoryFactory.GetPublicationStatus(newStory);

            if (newStatus == PublicationStatus.PUBLISHED) // moving to published story
                return SatisfiesPermission(StoryConstraint.HAS_ADMIN_ACCOUNT);
            else
                return true;
        }
    }
}
