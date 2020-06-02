using HourglassServer.Custom.User;
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

            string link = HtmlFormatters.GenerateLink("https://joinreach.org/passwordreset?token=" + token + "&email=" + to, "Reset here");

            string body = @"Follow this link to change your password:<br>" + link +
                           "<br><br>If you did not make a password change request, ignore this email.";

            var message =  new MailMessage(
                "reachcentralcoast@gmail.com",
                to,
                "Reach - Change your password",
                HtmlFormatters.BuildBodyFromTemplate(body));
            
            message.IsBodyHtml = true;
            return message;
        }
    }
}
