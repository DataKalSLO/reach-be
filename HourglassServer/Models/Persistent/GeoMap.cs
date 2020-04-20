using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GeoMap
    {
        public GeoMap()
        {
            GeoMapBlock = new HashSet<GeoMapBlock>();
            GeoMapTables = new HashSet<GeoMapTables>();
        }

        public string GeoMapId { get; set; }

        public virtual ICollection<GeoMapBlock> GeoMapBlock { get; set; }
        public virtual ICollection<GeoMapTables> GeoMapTables { get; set; }
    }
}
