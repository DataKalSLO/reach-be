using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphSourceModel
    {
        public string DatasetName { get; set; }
        public string[] ColumnNames { get; set; }
        public SeriesType SeriesType { get; set; }
    }
}
