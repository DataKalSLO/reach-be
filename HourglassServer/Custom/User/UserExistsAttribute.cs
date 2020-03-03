using Microsoft.AspNetCore.Authorization;

namespace HourglassServer
{
    public class UserExistsAttribute : AuthorizeAttribute
    {
        public UserExistsAttribute() : base()
        {
            Policy = "UserExists";
        }

    }
}
