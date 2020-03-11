using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Geomap
    {
        public Geomap()
        {
            GeoMapTables = new HashSet<GeoMapTables>();
            Geomapblock = new HashSet<Geomapblock>();
        }

        public string Geomapid { get; set; }

        public virtual ICollection<GeoMapTables> GeoMapTables { get; set; }
        public virtual ICollection<Geomapblock> Geomapblock { get; set; }
    }
}
