﻿using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text;
using HourglassServer.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace HourglassServer
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string BuildToken(Person person)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, person.Email),
                new Claim(ClaimTypes.Role, person.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireTime"])),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
