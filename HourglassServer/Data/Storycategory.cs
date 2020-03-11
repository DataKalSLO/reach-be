using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Storycategory
    {
        public string Storyid { get; set; }
        public string Categoryname { get; set; }

        public virtual Category CategorynameNavigation { get; set; }
        public virtual Story Story { get; set; }
    }
}
