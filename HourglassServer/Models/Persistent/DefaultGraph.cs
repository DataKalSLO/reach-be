using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class DefaultGraph
    {
        public string GraphId { get; set; }
        public string Category { get; set; }

        public virtual Graph Graph { get; set; }
    }
}
