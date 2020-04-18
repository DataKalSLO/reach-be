using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HourglassServer.Data;
using System;

namespace HourglassServer
{
    public class UserExistsHandler : AuthorizationHandler<UserExistsRequirement>, IAuthorizationRequirement
    {

        private HourglassContext _context;

        public UserExistsHandler(HourglassContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserExistsRequirement requirement)
        {
            // Check if the token has an email claim
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                context.Fail();
                return;
            }

            // Check if a user matching the email in the token exists
            try
            {
                string email = context.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;
                await _context.Person.SingleAsync(p => p.Email == email);
            }
            catch
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
    public class UserExistsRequirement : IAuthorizationRequirement
    {

    }
}
