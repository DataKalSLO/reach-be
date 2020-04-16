using System;
using System.ComponentModel.DataAnnotations;
using HourglassServer.Data.Persistent;
using Newtonsoft.Json;

namespace HourglassServer.Data.StoryModel
{
    public class StoryCreationObject
    {
        [Key]
        public String storyid { get; set; }
        public String userid { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        public StoryBlock[] storyblocks { get; set; }

        public override String ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
