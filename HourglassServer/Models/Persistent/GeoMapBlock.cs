using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class GeoMapBlock
    {
        public string BlockId { get; set; }
        public string StoryId { get; set; }
        public string GeoMapId { get; set; }
        public int BlockPosition { get; set; }

        public virtual GeoMap GeoMap { get; set; }
        public virtual Story Story { get; set; }
    }
}
