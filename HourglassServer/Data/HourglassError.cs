using System;
namespace HourglassServer.Data
{
    public class HourglassError
    {
        public HourglassError(string error, string action)
        {
            this.details = error;
            this.tag = action;
        }

        public string details { get; set; }
        public string tag { get; set; }
    }
}