using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class UniversityInfo
    {
        public int IdGender { get; set; }
        public string Gender { get; set; }
        public int IdUniversity { get; set; }
        public string University { get; set; }
        public int Completions { get; set; }
        public string County { get; set; }
        public string IdGeography { get; set; }
        public int Year { get; set; }
    }
}
