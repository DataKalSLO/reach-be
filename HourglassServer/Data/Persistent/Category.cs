using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
{
    public partial class Category
    {
        public Category()
        {
            StoryCategory = new HashSet<StoryCategory>();
        }

        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<StoryCategory> StoryCategory { get; set; }
    }
}
