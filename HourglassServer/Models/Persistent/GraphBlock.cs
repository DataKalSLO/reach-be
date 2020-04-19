using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GraphBlock
    {
        public string BlockId { get; set; }
        public string GraphId { get; set; }

        public virtual StoryBlock Block { get; set; }
        public virtual Graph Graph { get; set; }
    }
}
