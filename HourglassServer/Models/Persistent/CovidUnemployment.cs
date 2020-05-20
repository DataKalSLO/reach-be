using System;

namespace HourglassServer.Models.Persistent
{
    public partial class CovidUnemployment
    {
        public DateTime WeekEnd { get; set; }
        public int UnemploymentClaims { get; set; }
    }
}
