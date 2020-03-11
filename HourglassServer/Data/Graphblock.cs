using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Graphblock
    {
        public string Blockid { get; set; }
        public string Graphid { get; set; }

        public virtual BlockContent Block { get; set; }
        public virtual Graph Graph { get; set; }
    }
}
