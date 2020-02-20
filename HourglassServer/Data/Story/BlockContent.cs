using System;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Data.StoryModel
{
    public class BlockContent
    {
        [Key]
        public String id { get; set;  }
        public String editorstate { get; set; }
    }
}
