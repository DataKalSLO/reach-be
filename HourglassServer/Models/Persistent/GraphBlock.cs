using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GraphBlock
    {
        public string BlockId { get; set; }
        public string StoryId { get; set; }
        public string GraphId { get; set; }
        public int BlockPosition { get; set; }

        public virtual Graph Graph { get; set; }
        public virtual Story Story { get; set; }
    }
}
