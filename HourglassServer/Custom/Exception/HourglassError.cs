using System;
namespace HourglassServer.Data
{
    public class HourglassException: Exception
    {
        public HourglassException(string error, string tag)
        {
            this.details = error;
            this.tag = tag;
        }

        public string details { get; set; }
        public string tag { get; set; }
    }
}