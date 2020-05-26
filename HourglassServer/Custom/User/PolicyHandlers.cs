using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HourglassServer.Data;
using System;

namespace HourglassServer
{
    public static class PolicyHandlers
    {
        public static bool CheckValidPasswordResetToken(AuthorizationHandlerContext context)
        {
            // Check if the token has an email claim
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email) || !context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return false;
            }
            try
            {
                string role = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Single().Value;
                return role == "PasswordResetOnly";
            }
            catch
            {
                return false;
            }
        }
    }
}
