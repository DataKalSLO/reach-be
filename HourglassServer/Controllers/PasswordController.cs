using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Mail;


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

        public IActionResult Post([FromBody]string email)
        {
            string host = _configuration["Smtp:Host"];
            int port = 25;

            Person userWithEmail = _context.Person.First(p => p.Email == email);
            if (userWithEmail == null)
            {
                return Ok(); // Don't tell user email was invalid
            }

            string token = _jwtTokenService.BuildToken(userWithEmail);

            using (var client = new SmtpClient(host, port))
            {
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;

                try
                {
                    client.Send
                    (
                        "do-not-reply@reach-central-coast.com", // Sender address
                        email,
                        "Reach - Change your password",
                        @"Follow this link to change your password:
                        If you did not make a password change request, ignore this email.
                        https://joinreach.org/passwordreset?token=" + token
                    );
                }
                catch (SmtpFailedRecipientException e)
                {
                    return BadRequest(new { errorName = "failedEmailRecipient" });
                }
                catch (SmtpException e)
                {
                    return BadRequest(new { errorName = "smtpError" });
                }
            }

            return Ok();
        }
    }
}
