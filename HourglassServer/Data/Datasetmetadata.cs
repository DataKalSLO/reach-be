using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Datasetmetadata
    {
        public Datasetmetadata()
        {
            GeoMapTables = new HashSet<GeoMapTables>();
            Graphseries = new HashSet<Graphseries>();
            Location = new HashSet<Location>();
        }

        public string Tablename { get; set; }
        public string Columnnames { get; set; }
        public string Datatypes { get; set; }
        public bool[] Locationvalues { get; set; }
        public string[] Citycolumn { get; set; }
        public string[] Zipcodecolumn { get; set; }
        public string[] Countycolumn { get; set; }

        public virtual ICollection<GeoMapTables> GeoMapTables { get; set; }
        public virtual ICollection<Graphseries> Graphseries { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
