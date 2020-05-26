using HourglassServer.Models.Persistent;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HourglassServer.Custom.User
{
    public static class ClaimBuilders
    {
        private const string PasswordResetRole = "PasswordResetOnly";

        public static Claim[] BuildLoginClaims(Person person)
        {
            return new[]
            {
                new Claim(ClaimTypes.Email, person.Email),
                new Claim(ClaimTypes.Role, person.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        public static Claim[] BuildPasswordResetClaims(string email)
        {
            return new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, PasswordResetRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}
