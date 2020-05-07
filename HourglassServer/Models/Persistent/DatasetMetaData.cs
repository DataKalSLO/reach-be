using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class DatasetMetaData
    {
        public DatasetMetaData()
        {
            GeoMapTables = new HashSet<GeoMapTables>();
            GraphSeries = new HashSet<GraphSeries>();
            Location = new HashSet<Location>();
        }

        public string TableName { get; set; }
        public string[] ColumnNames { get; set; }
        public string[] DataTypes { get; set; }
        public string[] CityColumn { get; set; }
        public string[] ZipCodeColumn { get; set; }
        public string[] CountyColumn { get; set; }

        public virtual ICollection<GeoMapTables> GeoMapTables { get; set; }
        public virtual ICollection<GraphSeries> GraphSeries { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
