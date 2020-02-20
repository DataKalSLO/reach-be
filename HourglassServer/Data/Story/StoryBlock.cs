using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace HourglassServer.Data.StoryModel
{
    public class StoryBlock
    {
        [Key]
        public String blockid { get; set; }
        public int position { get; set; }
        public String type { get; set; }
        public BlockContent block { get; set; }
    }
}
