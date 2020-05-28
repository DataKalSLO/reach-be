using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using HourglassServer.Custom.User;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HourglassServer
{
    [DefaultControllerRoute]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private HourglassContext _context;

        public TokenController(IJwtTokenService jwtTokenService, HourglassContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TokenModel tokenModel)
        {
            Person loggedInUser;

            try
            {
                loggedInUser = await _context.Person.FirstAsync(p => p.Email == tokenModel.Email);
                if (loggedInUser.Salt == null ||
                    loggedInUser.PasswordHash == null ||
                    !UserPasswordHasher.PasswordMatches(tokenModel.Password, loggedInUser.Salt, loggedInUser.PasswordHash))
                {
                    return Unauthorized(new { tag = "badLogin" });
                }
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { tag = "badLogin" });
            }

            string token = _jwtTokenService.BuildToken(ClaimBuilders.BuildLoginClaims(loggedInUser));

            return Ok(new { email = tokenModel.Email, name = loggedInUser.Name, occupation = loggedInUser.Occupation, role = loggedInUser.Role, notificationsEnabled = loggedInUser.NotificationsEnabled, loggedInUser.IsThirdParty, token });
        }
    }
}
