using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class CensusVariables
    {
        public CensusVariables()
        {
            CensusData = new HashSet<CensusData>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CensusData> CensusData { get; set; }
    }
}
