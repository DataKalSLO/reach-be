using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Location
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public int? PointId { get; set; }

        public virtual Point Point { get; set; }
        public virtual Datasetmetadata TableNameNavigation { get; set; }
    }
}
