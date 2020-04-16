using System;
using System.ComponentModel.DataAnnotations;
using HourglassServer.Data.Persistent;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


/* Responsibility: Represent the front end interface Story. This is the object
 * format expected in the the API routes.
 */
namespace HourglassServer.Data.Application.StoryModel
{
    public class StoryBlockModel : IComparable<StoryBlockModel>
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))] //Strings not converted by default
        public StoryBlockType Type { get; set; } 

        public string GraphId { get; set; }

        public string MapId { get; set; }

        public string EditorState { get; set; }

        /* Note, FEND will not define this and will be null after initial model binding
         * This means that this is required to be set based on its position in
         * given Story's storyBlocks
         */
        public int BlockPosition { get; set; }

        public StoryBlockModel() { }

        public StoryBlockModel(GeoMapBlock mapBlock, int position)
        {
            this.Id = mapBlock.BlockId;
            this.Type = StoryBlockType.GEOMAP;
            this.MapId = mapBlock.GeoMapId;
            this.BlockPosition = position;
        }

        public StoryBlockModel(GraphBlock graphBlock, int position)
        {
            this.Id = graphBlock.BlockId;
            this.Type = StoryBlockType.GRAPH;
            this.GraphId = graphBlock.GraphId;
            this.BlockPosition = position;
        }

        public StoryBlockModel(TextBlock textBlock, int position)
        {
            this.Id = textBlock.BlockId;
            this.Type = StoryBlockType.TEXTDB;
            this.EditorState = textBlock.EditorState;
            this.BlockPosition = position;
        }

        public int CompareTo(StoryBlockModel other)
        {
            if (this.BlockPosition < other.BlockPosition)
                return -1;
            else if (this.BlockPosition > other.BlockPosition)
                return 1;
            else
                return 0;
        }
    }
}
