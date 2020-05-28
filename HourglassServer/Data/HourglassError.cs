using System;
namespace HourglassServer.Data
{
    public class HourglassError: Exception
    {
        public HourglassError(string error, string tag)
        {
            this.details = error;
            this.tag = tag;
        }

        public string details { get; set; }
        public string tag { get; set; }
    }
}