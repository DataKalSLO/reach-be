using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GeoMapTables
    {
        public string GeoMapId { get; set; }
        public string TableName { get; set; }

        public virtual GeoMap GeoMap { get; set; }
        public virtual DatasetMetaData TableNameNavigation { get; set; }
    }
}
