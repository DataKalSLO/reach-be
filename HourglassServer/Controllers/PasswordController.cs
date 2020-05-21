﻿using HourglassServer.Custom.User;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer
{
    [DefaultControllerRoute]
    public class PasswordController : ControllerBase
    {
        private HourglassContext _context;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;

        public PasswordController(HourglassContext context, IConfiguration configuration,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmailModel model)
        {
            string host = _configuration["Smtp:Host"];
            int port = 25;

            string token = _jwtTokenService.BuildToken(ClaimBuilders.BuildPasswordResetClaims(model.Email));

            using (var client = new SmtpClient(host, port))
            {
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;

                var toSend = new MailMessage(
                    "reachcentralcoast@gmail.com",
                    model.Email,
                    "Reach - Change your password",
                    @"Follow this link to change your password:
If you did not make a password change request, ignore this email.
https://joinreach.org/passwordreset?token=" + token + "&email=" + model.Email
                );

                try
                {
                    await client.SendMailAsync(toSend);
                }
                catch (SmtpFailedRecipientException)
                {
                    return BadRequest(new { tag = "failedEmailRecipient" });
                }
                catch (SmtpException)
                {
                    return BadRequest(new { tag = "smtpError" });
                }
            }

            return Ok();
        }

        [Authorize(Policy = "ValidPasswordResetToken")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PasswordChangeModel model) // enforce user has permissions
        {
            Person person;
            try
            {
                person = await _context.Person.SingleAsync(p => p.Email == model.Email);
            }
            catch
            {
                return NotFound();
            }

            var (salt, hash) = Utilities.HashPassword(model.Password); // enforce password restrictions
            person.Salt = salt;
            person.PasswordHash = hash;

            await _context.UpdateAsync(person);
            return Ok();
        }
    }
}
