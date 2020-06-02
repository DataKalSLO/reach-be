
namespace HourglassServer.Custom.Constraints
{
    public enum Constraints
    {
        HAS_USER_ACCOUNT,
        HAS_ADMIN_ACCOUNT,
        AUTHORIZED_USER_OWNS_STORY,
        HAS_STORY_OWNERSHIP_OR_HAS_ADMIN_ACCOUNT,
        HAS_DRAFT_STATUS,
        HAS_PERMISSION_TO_CHANGE_STATUS,
        STORY_EXISTS_WITH_ID
    }
}
