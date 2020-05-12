using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Story
    {
        public Story()
        {
            BookmarkStory = new HashSet<BookmarkStory>();
            StoryBlock = new HashSet<StoryBlock>();
            StoryCategory = new HashSet<StoryCategory>();
        }

        public string StoryId { get; set; }
        public string UserId { get; set; }
        public string PublicationStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }

        public virtual Person User { get; set; }
        public virtual ICollection<BookmarkStory> BookmarkStory { get; set; }
        public virtual ICollection<StoryBlock> StoryBlock { get; set; }
        public virtual ICollection<StoryCategory> StoryCategory { get; set; }
    }
}
