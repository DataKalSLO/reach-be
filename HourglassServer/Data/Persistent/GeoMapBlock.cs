using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
{
    public partial class GeoMapBlock
    {
        public string BlockId { get; set; }
        public string GeoMapId { get; set; }

        public virtual StoryBlock Block { get; set; }
        public virtual GeoMap GeoMap { get; set; }
    }
}
