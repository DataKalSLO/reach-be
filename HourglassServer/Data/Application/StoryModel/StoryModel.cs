using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using HourglassServer.Data.Persistent;

/* Represents the Application Model (FEND) version of Story.
 * Note, the reason there are two versions Application vs Persistent is because
 * to normalize the data surrounding a Story we end up separating Story into
 * its independant subparts.
 *
 * StoryModel -> Story + StoryBlockModel[]
 * StoryBlockModel -> StoryBlock + <TypeBlock>
 *
 * where <TypeBlock> is one of GraphBlock, TextBlock, or GeoMapBlock
 */
namespace HourglassServer.Data.Application.StoryModel
{
    public class StoryModel
    {
        [Key]
        [Required]
        public string id { get; set; }

        [Required]
        public string userID { get; set; }

        [Required]
        public string title { get; set; }

        public string description { get; set; }

        public string publicationStatus { get; set; }

        [Required]
        public List<StoryBlockModel> storyBlocks { get; set; }

        public StoryModel()
        {
            this.publicationStatus = "DRAFT"; //TODO: Find a way to make constant (enum or something else)
        }

        public StoryModel(Story story)
        {
            this.id = story.StoryId;
            this.userID = story.UserId;
            this.title = story.Title;
            this.description = story.Description;
            this.publicationStatus = story.PublicationStatus;
        }
    }
}
