using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using HourglassServer.Data.Persistent;
using Newtonsoft.Json.Converters;

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
    public class StoryApplicationModel
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PublicationStatus PublicationStatus { get; set; }

        [Required]
        public List<StoryBlockModel> StoryBlocks { get; set; }

        public StoryApplicationModel()
        {
            this.PublicationStatus = PublicationStatus.DRAFT; //TODO: Find a way to make constant (enum or something else)
        }

        public StoryApplicationModel(Story story)
        {
            this.Id = story.StoryId;
            this.UserId = story.UserId;
            this.Title = story.Title;
            this.Description = story.Description;
            this.PublicationStatus = (PublicationStatus) Enum.Parse(typeof(PublicationStatus), story.PublicationStatus, true);
        }
    }
}
