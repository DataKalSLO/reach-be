using System.Security.Claims;
using HourglassServer.Data;

namespace HourglassServer.Custom.Constraints
{
    public class ConstraintEnvironment
    {
        public ConstraintEnvironment(ClaimsPrincipal user, HourglassContext context)
        {
            this.user = user;
            this.context = context;
        }

        public ClaimsPrincipal user { get; set; }
        public HourglassContext context { get; set; }
    }
}
