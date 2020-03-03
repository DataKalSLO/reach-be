using Microsoft.AspNetCore.Authorization;

namespace HourglassServer
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(Role role) : base()
        {
            switch (role)
            {
                case Role.BaseUser:
                    Roles = "BaseUser";
                    break;
                case Role.Admin:
                    Roles = "Admin";
                    break;
            }
        }
    }
}
