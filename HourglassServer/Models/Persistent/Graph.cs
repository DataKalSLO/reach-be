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
            GraphSource = new HashSet<GraphSource>();
        }

        public string GraphId { get; set; }
        public string GraphTitle { get; set; }
        public string UserId { get; set; }
        public long? Timestamp { get; set; }
        public string SnapshotUrl { get; set; }
        public string GraphOptions { get; set; }

        public virtual Person User { get; set; }
        public virtual ICollection<BookmarkGraph> BookmarkGraph { get; set; }
        public virtual ICollection<GraphBlock> GraphBlock { get; set; }
        public virtual ICollection<GraphSource> GraphSource { get; set; }
    }
}
