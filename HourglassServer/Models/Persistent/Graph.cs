using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Graph
    {
        public Graph()
        {
            GraphBlock = new HashSet<GraphBlock>();
            GraphBookmark = new HashSet<GraphBookmark>();
            GraphSeries = new HashSet<GraphSeries>();
        }

        public string GraphId { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }

        public virtual Person User { get; set; }
        public virtual ICollection<GraphBlock> GraphBlock { get; set; }
        public virtual ICollection<GraphBookmark> GraphBookmark { get; set; }
        public virtual ICollection<GraphSeries> GraphSeries { get; set; }
    }
}
