using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class SloAirport
    {
        public DateTime Month { get; set; }
        public int Alaska { get; set; }
        public int American { get; set; }
        public int Contour { get; set; }
        public int United { get; set; }
        public int GrandTotal2019 { get; set; }
        public int GrandTotal2018 { get; set; }
        public decimal PctChange { get; set; }
    }
}
