﻿using HourglassServer.Custom.User;
using System.Net.Mail;

namespace HourglassServer.Mail
{
    public class EmailService : IEmailService
    {
        public const string ReachEmail = "reachcentralcoast@gmail.com";

        private readonly IJwtTokenService _jwtTokenService;

        public EmailService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public MailMessage GeneratePasswordEmail(string to)
        {
            string token = _jwtTokenService.BuildToken(ClaimBuilders.BuildPasswordResetClaims(to));

            return new MailMessage(
                "reachcentralcoast@gmail.com",
                to,
                "Reach - Change your password",
                @"Follow this link to change your password:
If you did not make a password change request, ignore this email.
https://joinreach.org/passwordreset?token=" + token + "&email=" + to
                );
        }
    }
}
