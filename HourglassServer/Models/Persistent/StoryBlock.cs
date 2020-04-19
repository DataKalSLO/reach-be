using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class StoryBlock
    {
        public string BlockId { get; set; }
        public string StoryId { get; set; }
        public int BlockPosition { get; set; }

        public virtual Story Story { get; set; }
        public virtual GeoMapBlock GeoMapBlock { get; set; }
        public virtual GraphBlock GraphBlock { get; set; }
        public virtual TextBlock TextBlock { get; set; }
    }
}
