using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Point
    {
        public int Id { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
