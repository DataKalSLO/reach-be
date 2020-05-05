using HourglassServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer
{
    [DefaultControllerRoute]
    public class PasswordController : ControllerBase
    {
        private HourglassContext _context;
        private readonly IConfiguration _configuration;

        public PasswordController(HourglassContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Post([FromBody]string email)
        {
            string host = _configuration["Smtp:Host"];
            int port = 25;

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
                        "Follow this link to change your password: \nIf you did not make a password change request, ignore this email."
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
