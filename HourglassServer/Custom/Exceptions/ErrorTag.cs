using System;
namespace HourglassServer.Custom.Exceptions
{
    public class ErrorTag
    {
        public const string badValueTag = "badValue";
        public const string nowOwnerTag = "notOwner";
        public const string forbiddenRoleTag = "forbiddenRole";
        public const string queryFailedTag = "queryFailed";
    }
}
