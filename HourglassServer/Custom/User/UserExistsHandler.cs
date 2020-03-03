using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HourglassServer.Data;

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
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                context.Fail();
                return;
            }
            try
            {
                await _context.Person.SingleAsync(p => p.Email == context.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value);
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
