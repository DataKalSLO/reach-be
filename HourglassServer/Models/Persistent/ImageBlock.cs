using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class ImageBlock
    {
        public string StoryId { get; set; }
        public string BlockId { get; set; }
        public int BlockPosition { get; set; }
        public string ImageUrl { get; set; }

        public virtual Story Story { get; set; }
    }
}
