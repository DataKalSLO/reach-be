using System;
namespace HourglassServer.Custom.Exceptions
{
    public class ErrorTag
    {
        public const string BadValue = "badValue";
        public const string NotOwner = "notOwner";
        public const string ForbiddenRole = "forbiddenRole";
        public const string QueryFailed = "queryFailed";
    }
}
