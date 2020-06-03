using System.Net.Mail;

namespace HourglassServer.Mail
{
    public interface IEmailService
    {
        MailMessage GeneratePasswordEmail(string to);
        void SendMail(MailMessage message);
    }
}
