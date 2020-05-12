﻿using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Story
    {
        public Story()
        {
            BookmarkStory = new HashSet<BookmarkStory>();            
            GeoMapBlock = new HashSet<GeoMapBlock>();
            GraphBlock = new HashSet<GraphBlock>();
            StoryCategory = new HashSet<StoryCategory>();
            TextBlock = new HashSet<TextBlock>();
        }

        public string StoryId { get; set; }
        public string UserId { get; set; }
        public string PublicationStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }

        public virtual Person User { get; set; }
        public virtual ICollection<GeoMapBlock> GeoMapBlock { get; set; }
        public virtual ICollection<GraphBlock> GraphBlock { get; set; }
<<<<<<< HEAD
        public virtual ICollection<StoryBlock> StoryBlock { get; set; }
=======
        public virtual ICollection<BookmarkStory> BookmarkStory { get; set; }
>>>>>>> 34639d374c6806d331e1fd94b89794928292641f
        public virtual ICollection<StoryCategory> StoryCategory { get; set; }
        public virtual ICollection<TextBlock> TextBlock { get; set; }
    }
}
