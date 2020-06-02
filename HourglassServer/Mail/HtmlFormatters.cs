using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HourglassServer.Mail
{
    public static class HtmlFormatters
    {
        public static string BuildBodyFromTemplate(string messageBody)
        {
            using StreamReader reader = File.OpenText("Mail\\email_template.html");
            string body = reader.ReadToEnd();
            return body.Replace("{{MESSAGE_BODY}}", messageBody); ;
        }
        public static string GenerateLink(string url, string text)
        {
            return $"<a href=\"{url}\">{text}</a>";
        }
    }
}
