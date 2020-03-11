using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Graphseries
    {
        public string Graphid { get; set; }
        public string Tablename { get; set; }
        public string Columnname { get; set; }
        public string Seriestype { get; set; }

        public virtual Graph Graph { get; set; }
        public virtual Datasetmetadata TablenameNavigation { get; set; }
    }
}
