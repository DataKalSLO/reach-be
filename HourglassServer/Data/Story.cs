using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Story
    {
        public Story()
        {
            Storyblock = new HashSet<Storyblock>();
            Storycategory = new HashSet<Storycategory>();
        }

        public string Storyid { get; set; }
        public string Userid { get; set; }
        public string Publicationstatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datelastedited { get; set; }

        public virtual Hourglassuser User { get; set; }
        public virtual ICollection<Storyblock> Storyblock { get; set; }
        public virtual ICollection<Storycategory> Storycategory { get; set; }
    }
}
