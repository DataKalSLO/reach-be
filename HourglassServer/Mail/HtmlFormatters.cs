﻿using System.IO;

namespace HourglassServer.Mail
{
    public static class HtmlFormatters
    {
        private const string emailTemplate = "email_template.html";

        public static string BuildBodyFromTemplate(string messageBody)
        {
            using StreamReader reader = File.OpenText($"..{Path.DirectorySeparatorChar}"
                + $"Config{Path.DirectorySeparatorChar}{emailTemplate}");
            string body = reader.ReadToEnd();
            return body.Replace("{{MESSAGE_BODY}}", messageBody); ;
        }
        public static string GenerateLink(string url, string text)
        {
            return $"<a href=\"{url}\">{text}</a>";
        }
    }
}
