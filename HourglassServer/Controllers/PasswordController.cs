using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.Extensions.Configuration;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer
{
    [DefaultControllerRoute]
    public class PasswordController : ControllerBase
    {
        private HourglassContext _context;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public PasswordController(HourglassContext context, ILogger<PersonController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPut("{email}")]
        public void Put(string email, [FromBody]string value)
        {
            string host = _configuration["Smtp:Host"];
            int port = 25;

            using (var client = new System.Net.Mail.SmtpClient(host, port))
            {
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;

                client.Send
                (
                    "do-not-reply@reach-central-coast.com", // Sender address
                    email,
                    "Reach - Change your password",
                    "Follow this link to change your password: \nIf you did not make a password change request, ignore this email."
                );
            }
        }
    }
}
