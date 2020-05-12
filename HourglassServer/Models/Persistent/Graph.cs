using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Graph
    {
        public Graph()
        {
            BookmarkGraph = new HashSet<BookmarkGraph>();
            GraphBlock = new HashSet<GraphBlock>();
            GraphSeries = new HashSet<GraphSeries>();
        }

        public string GraphId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<BookmarkGraph> BookmarkGraph { get; set; }
        public virtual ICollection<GraphBlock> GraphBlock { get; set; }
        public virtual ICollection<GraphSeries> GraphSeries { get; set; }
    }
}
