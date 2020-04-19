using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class StoryCategory
    {
        public string StoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual Category CategoryNameNavigation { get; set; }
        public virtual Story Story { get; set; }
    }
}
