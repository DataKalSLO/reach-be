using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class GeoMapTables
    {
        public string GeoMapId { get; set; }
        public string TableName { get; set; }

        public virtual Geomap GeoMap { get; set; }
        public virtual Datasetmetadata TableNameNavigation { get; set; }
    }
}
