using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GraphSources
    {
        public string GraphId { get; set; }
        public string SeriesType { get; set; }
        public string DatasetName { get; set; }
        public string ColumnNames { get; set; }

        public virtual DatasetMetaData DatasetNameNavigation { get; set; }
        public virtual Graph Graph { get; set; }
    }
}
