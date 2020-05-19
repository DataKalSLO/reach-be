using System;
namespace HourglassServer.Custom.Exceptions
{
    public class ErrorTag
    {
        public const string badValue = "badValue";
        public const string nowOwner = "notOwner";
        public const string forbiddenRole = "forbiddenRole";
        public const string queryFailed = "queryFailed";
    }
}
