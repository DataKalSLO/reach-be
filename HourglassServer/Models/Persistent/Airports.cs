using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Airports
    {
        public string Code { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Name { get; set; }
    }
}
