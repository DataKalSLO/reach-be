using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HourglassServer.Data.StoryModel
{
    public class Story
    {
        [Key]
        public String storyid { get; set; }
        public String userid { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        public DateTime datecreated { get; set;  }
        public DateTime datelastedited { get; set; }

        public override String ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void setStateFromCreationObject(StoryCreationObject obj)
        {
            this.storyid = obj.storyid;
            this.userid = obj.userid;
            this.title = obj.title;
            this.description = obj.description;
            this.datecreated = DateTime.Now;
            this.datelastedited = DateTime.Now;
        }
    }
}
