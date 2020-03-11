using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Storyblock
    {
        public string Blockid { get; set; }
        public string Storyid { get; set; }
        public int Blockposition { get; set; }

        public virtual Story Story { get; set; }
        public virtual Geomapblock Geomapblock { get; set; }
        public virtual Graphblock Graphblock { get; set; }
        public virtual Textblock Textblock { get; set; }
    }
}
