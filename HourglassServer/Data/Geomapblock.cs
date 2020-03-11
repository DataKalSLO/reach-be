using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Geomapblock
    {
        public string Blockid { get; set; }
        public string Geomapid { get; set; }

        public virtual BlockContent Block { get; set; }
        public virtual Geomap Geomap { get; set; }
    }
}
