using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class CensusData
    {
        public string VariableName { get; set; }
        public int Year { get; set; }
        public string GeoName { get; set; }
        public GeoType GeoType { get; set; } 
        public int? Value { get; set; }

        public virtual CensusVariables VariableNameNavigation { get; set; }
    }
}
