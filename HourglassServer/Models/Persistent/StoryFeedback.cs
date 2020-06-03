using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class StoryFeedback
    {
        public string FeedbackId { get; set; }
        public string StoryId { get; set; }
        public string ReviewerId { get; set; }
        public string Feedback { get; set; }

        public virtual Story Story { get; set; }
    }
}
