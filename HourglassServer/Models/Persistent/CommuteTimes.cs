using System;

namespace HourglassServer.Models.Persistent
{
    public partial class CommuteTimes
    {
        public DateTime Year { get; set; }
        public decimal AvgMinutes { get; set; }
        public string City { get; set; }
    }
}
