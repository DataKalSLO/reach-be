using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Area
    {
        public string Name { get; set; }
        public int PointId { get; set; }

        public virtual Point Point { get; set; }
    }
}
