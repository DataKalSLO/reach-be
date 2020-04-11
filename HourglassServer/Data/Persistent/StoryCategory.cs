using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
{
    public partial class StoryCategory
    {
        public string StoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual Category CategoryNameNavigation { get; set; }
        public virtual Story Story { get; set; }
    }
}
