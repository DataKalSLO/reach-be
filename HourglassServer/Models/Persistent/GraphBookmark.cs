using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GraphBookmark
    {
        public string UserId { get; set; }
        public string GraphId { get; set; }

        public virtual Graph Graph { get; set; }
    }
}
