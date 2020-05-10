using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class TextBlock
    {
        public string BlockId { get; set; }
        public string StoryId { get; set; }
        public string EditorState { get; set; }
        public int BlockPosition { get; set; }

        public virtual Story Story { get; set; }
    }
}
