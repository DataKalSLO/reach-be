using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using HourglassServer.Data;
using System.Collections.Generic;

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
        public IActionResult Post([FromBody]TokenModel tokenModel)
        {
            Person loggedInUser;

            try
            {
                loggedInUser = _context.Person.First(p => p.Email == tokenModel.Email
                    && p.Password == tokenModel.Password);
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { tag = "badLogin" });
            }

            string token = _jwtTokenService.BuildToken(loggedInUser);

            return Ok(new { email = tokenModel.Email, token });
        }
    }
}
