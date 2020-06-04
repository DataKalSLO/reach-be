using HourglassServer.Models.Persistent;
using System.Net.Mail;

namespace HourglassServer.Mail
{
    public interface IEmailService
    {
        MailMessage GenerateStatusUpdateEmail(Person user, string title, string publicationStatus);
        MailMessage GeneratePasswordEmail(string to);
        void SendMail(MailMessage message);
    }
}
