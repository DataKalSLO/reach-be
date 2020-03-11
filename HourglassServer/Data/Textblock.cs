using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Textblock
    {
        public string Blockid { get; set; }
        public string Editorstate { get; set; }

        public virtual BlockContent Block { get; set; }
    }
}
