using System;
using System.Collections.Generic;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Data
{
    public partial class Hourglassuser
    {
        public Hourglassuser()
        {
            Story = new HashSet<Story>();
        }

        public string Userid { get; set; }

        public virtual ICollection<Story> Story { get; set; }
    }
}
