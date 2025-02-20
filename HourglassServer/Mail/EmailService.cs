﻿using HourglassServer.Custom.User;
using HourglassServer.Models.Persistent;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace HourglassServer.Mail
{
    public class EmailService : IEmailService
    {
        public const string ReachEmail = "reachcentralcoast@gmail.com";

        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public EmailService(IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public MailMessage GeneratePasswordEmail(string to)
        {
            string token = _jwtTokenService.BuildToken(ClaimBuilders.BuildPasswordResetClaims(to));

            string link = HtmlFormatters.GenerateLink("https://www.joinreach.org/passwordreset?token=" + token + "&email=" + to, "Reset here");

            string body = @"Follow this link to change your password:<br>" + link +
                           "<br><br>If you did not make a password change request, ignore this email.";

            var message = new MailMessage(
                ReachEmail,
                to,
                "Reach - Change your password",
                HtmlFormatters.BuildBodyFromTemplate(body));

            message.IsBodyHtml = true;
            return message;
        }

        public MailMessage GenerateStatusUpdateEmail(Person user, string title, string publicationStatus)
        {
            string body = $"Hi {user.Name},<br><br>The status of your story <strong>{title}</strong>" +
                          $" has been updated to <strong>{publicationStatus}</strong>.";

            var message = new MailMessage(
                ReachEmail,
                user.Email,
                "Reach - Your story status has changed",
                HtmlFormatters.BuildBodyFromTemplate(body));

            message.IsBodyHtml = true;
            return message;
        }

        public async void SendMail(MailMessage toSend)
        {
            string host = _configuration["Smtp:Host"];
            int port = 587;

            using var client = new SmtpClient(host, port);
            var username = _configuration["Smtp:Username"];
            var password = _configuration["Smtp:Password"];

            client.Credentials = new System.Net.NetworkCredential(username, password);
            client.EnableSsl = true;


            toSend.Headers.Add("X-SES_CONFIGURATION-SET", "ConfigSet");


            await client.SendMailAsync(toSend);
        }
    }
}
