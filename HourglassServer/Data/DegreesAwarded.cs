using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class DegreesAwarded
    {
        public int Year { get; set; }
        public string IdCounty { get; set; }
        public int? TotalPopulation { get; set; }
        public int? NumNoDiploma { get; set; }
        public int? HsDiploma { get; set; }
        public int? Associates { get; set; }
        public int? Bachelor { get; set; }
    }
}
