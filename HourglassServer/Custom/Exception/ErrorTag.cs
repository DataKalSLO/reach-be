using System;
namespace HourglassServer.Custom.Exception
{
    public class ExceptionTag
    {
        public const string BadValue = "badValue";
        public const string NotOwner = "notOwner";
        public const string ForbiddenRole = "forbiddenRole";
        public const string QueryFailed = "queryFailed";
        public const string NotFound = "notFound";
    }
}
