using HourglassServer.Custom.User;
using HourglassServer.Data;
using HourglassServer.Mail;
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
        private readonly IEmailService _emailService;

        public PasswordController(HourglassContext context, IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmailModel model)
        {
            string host = _configuration["Smtp:Host"];
            int port = 25;

            using (var client = new SmtpClient(host, port))
            {
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;

                var toSend = _emailService.GeneratePasswordEmail(model.Email);

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
        public async Task<IActionResult> Put([FromBody]PasswordChangeModel model)
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

            var (salt, hash) = UserPasswordHasher.HashPassword(model.Password);
            person.Salt = salt;
            person.PasswordHash = hash;

            await _context.UpdateAsync(person);
            return Ok();
        }
    }
}
