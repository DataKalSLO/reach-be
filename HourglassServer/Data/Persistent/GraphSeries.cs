using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
{
    public partial class GraphSeries
    {
        public string GraphId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string SeriesType { get; set; }

        public virtual Graph Graph { get; set; }
        public virtual DatasetMetaData TableNameNavigation { get; set; }
    }
}
