﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using HourglassServer.Custom.User;

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
                Person userWithEmail = _context.Person.First(p => p.Email == tokenModel.Email);
                if (userWithEmail.Salt == null ||
                    userWithEmail.PasswordHash == null ||
                    !Utilities.PasswordMatches(tokenModel.Password, userWithEmail.Salt, userWithEmail.PasswordHash))
                {
                    return Unauthorized(new { tag = "badLogin" });
                }

                loggedInUser = userWithEmail;
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { tag = "badLogin" });
            }

            string token = _jwtTokenService.BuildToken(ClaimBuilders.BuildLoginClaims(loggedInUser));

            return Ok(new { email = tokenModel.Email, token });
        }
    }
}
