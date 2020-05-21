using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class DatasetMetaData
    {
        public DatasetMetaData()
        {
            GeoMapTables = new HashSet<GeoMapTables>();
            GraphSource = new HashSet<GraphSource>();
            Location = new HashSet<Location>();
        }

        public string TableName { get; set; }
        public string[] ColumnNames { get; set; }
        public string[] DataTypes { get; set; }
        public string GeoType { get; set; }

        public virtual ICollection<GeoMapTables> GeoMapTables { get; set; }
        public virtual ICollection<GraphSource> GraphSource { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
