using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class BookmarkStory
    {
        public string UserId { get; set; }
        public string StoryId { get; set; }

        public virtual Story Story { get; set; }
    }
}
