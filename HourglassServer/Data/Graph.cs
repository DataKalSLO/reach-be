using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Graph
    {
        public Graph()
        {
            Graphblock = new HashSet<Graphblock>();
            Graphseries = new HashSet<Graphseries>();
        }

        public string Graphid { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Graphblock> Graphblock { get; set; }
        public virtual ICollection<Graphseries> Graphseries { get; set; }
    }
}
