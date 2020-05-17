using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Custom.StoryModel
{
    enum StoryActionConstraint
    {
        HAS_USER_ACCOUNT,
        HAS_ADMIN_ACCOUNT,
        STORY_USER_MATCHES_AUTHORIZED_USER,
        AUTHORIZED_USER_OWNS_STORY,
        HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
        HAS_DRAFT_STATUS,
        HAS_PERMISSION_TO_CHANGE_STATUS,
        STORY_EXISTS_WITH_ID
    }

    class StoryPermissionCheckers
    {
        public const string badValueTag = "badValue";
        public const string nowOwnerTag = "notOwner";
        public const string forbiddenRoleTag = "forbiddenRole";
        public const string queryFailedTag = "queryFailed";

        public delegate bool permission(ClaimsPrincipal user, HourglassContext context, Story newStory);

        private Dictionary<StoryActionConstraint, permission> permissions = new Dictionary<StoryActionConstraint, permission>();
        private Dictionary<StoryActionConstraint, (string message, string tag)> permissionErrors = new Dictionary<StoryActionConstraint, (string message, string tag)>();
        private ClaimsPrincipal user;
        private Story newStory;
        private HourglassContext context;

        public StoryPermissionCheckers(ClaimsPrincipal user, HourglassContext context)
        {
            this.user = user;
            this.context = context;
            this.newStory = null;
            CreatePermissions();
        }

        public StoryPermissionCheckers(ClaimsPrincipal user, HourglassContext context, StoryApplicationModel newStory) : this(user, context)
        {
            this.newStory = StoryFactory.CreateStoryFromStoryModel(newStory);
        }

        public StoryPermissionCheckers(ClaimsPrincipal user, HourglassContext context, Story newStory) : this(user, context)
        {
            this.newStory = newStory;
        }

        private void CreatePermissions()
        {
            permissions.Add(StoryActionConstraint.HAS_USER_ACCOUNT, (user, context, newStory) => context.Person.Any(p => p.Email == user.GetUserId()));
            permissionErrors.Add(StoryActionConstraint.HAS_USER_ACCOUNT, ("Account required for action.", forbiddenRoleTag));

            permissions.Add(StoryActionConstraint.HAS_ADMIN_ACCOUNT, (user, context, newStory) => user.HasRole(Role.Admin));
            permissionErrors.Add(StoryActionConstraint.HAS_ADMIN_ACCOUNT, ("Administrator account required for action.", forbiddenRoleTag));

            permissions.Add(StoryActionConstraint.AUTHORIZED_USER_OWNS_STORY, HasOwnershipOfStory);
            permissionErrors.Add(StoryActionConstraint.AUTHORIZED_USER_OWNS_STORY, ("Authorized user is not owner of story", nowOwnerTag));

            permissions.Add(StoryActionConstraint.STORY_USER_MATCHES_AUTHORIZED_USER, (user, context, newStory) => newStory.UserId == user.GetUserId());
            permissionErrors.Add(StoryActionConstraint.STORY_USER_MATCHES_AUTHORIZED_USER, ("Authorized user is not owner of story", nowOwnerTag));

            permissions.Add(StoryActionConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
                (user, context, newStory) => SatisfiesAtLeastOnePermission(new StoryActionConstraint[] { StoryActionConstraint.AUTHORIZED_USER_OWNS_STORY, StoryActionConstraint.HAS_ADMIN_ACCOUNT }));
            permissionErrors.Add(StoryActionConstraint.HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT, ("Authorized user is not owner of story or an administrator.", forbiddenRoleTag));

            permissions.Add(StoryActionConstraint.HAS_DRAFT_STATUS, (user, context, newStory) => StoryFactory.GetPublicationStatus(newStory) == PublicationStatus.DRAFT);
            permissionErrors.Add(StoryActionConstraint.HAS_DRAFT_STATUS, ("Action requires Story to be in DRAFT status.", badValueTag));

            permissions.Add(StoryActionConstraint.HAS_PERMISSION_TO_CHANGE_STATUS, HasPermissionToChangeStatus);
            permissionErrors.Add(StoryActionConstraint.HAS_PERMISSION_TO_CHANGE_STATUS, ("User not permitted to update the status of this story.", forbiddenRoleTag));

            permissions.Add(StoryActionConstraint.STORY_EXISTS_WITH_ID, (user, context, newStory) => context.Story.Any(story => story.StoryId == newStory.StoryId));
            permissionErrors.Add(StoryActionConstraint.STORY_EXISTS_WITH_ID, ("Could not find an instance of given story.", queryFailedTag));
        }

        public void AssertAtLeastOnePermission(StoryActionConstraint[] constraints)
        {
            if (!SatisfiesAtLeastOnePermission(constraints))
                ThrowPermissionException(constraints[0]);
        }

        public void AssertAllPermissions(StoryActionConstraint[] constraints)
        {
            (bool result, StoryActionConstraint? potentialFailingAction) = SatisfiesAllPermissions(constraints);
            if (potentialFailingAction is StoryActionConstraint failingAction)
                ThrowPermissionException(failingAction);
        }

        public void AssertPermission(StoryActionConstraint action)
        {
            if (!this.SatisfiesPermission(action))
                ThrowPermissionException(action);
        }

        public void AssertPermission(permission action, string message, string tag)
        {
            if (!this.SatisfiesPermission(action))
                ThrowPermissionException(message, tag);
        }

        public void ThrowPermissionException(StoryActionConstraint action)
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

        public bool SatisfiesAtLeastOnePermission(StoryActionConstraint[] constraints)
        {
            if (constraints.Count() == 0)
                throw new InvalidOperationException("Expected at least one constraint.");
            foreach (StoryActionConstraint constraint in constraints)
            {
                if (this.SatisfiesPermission(constraint))
                    return true;
            }
            return false;
        }

        public (bool result, StoryActionConstraint? fail) SatisfiesAllPermissions(StoryActionConstraint[] constraints)
        {
            if (constraints.Count() == 0)
                throw new InvalidOperationException("Expected at least one constraint.");
            foreach (StoryActionConstraint constraint in constraints)
            {
                if (!this.SatisfiesPermission(constraint))
                    return (false, constraint);
            }
            return (true, null);
        }

        public bool SatisfiesPermission(StoryActionConstraint action)
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
            AssertPermission(StoryActionConstraint.HAS_USER_ACCOUNT);
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
                return SatisfiesPermission(StoryActionConstraint.HAS_ADMIN_ACCOUNT);
            else
                return true;
        }
    }
}
