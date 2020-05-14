using System;
using System.ComponentModel.DataAnnotations;
using HourglassServer.Models.Persistent;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


/* Responsibility: Represent the front end interface Story. This is the object
 * format expected in the the API routes.
 */
namespace HourglassServer.Data.Application.StoryModel
{
    /*TODO: Make this an abstract class with child StoryBlocks (e.g. TextBlock)
     * and create custom binder to redirect to child constructor.
     */
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

        public StoryBlockModel(GeoMapBlock mapBlock)
        {
            this.Id = mapBlock.BlockId;
            this.Type = StoryBlockType.GEOMAP;
            this.MapId = mapBlock.GeoMapId;
            this.BlockPosition = mapBlock.BlockPosition;
        }

        public StoryBlockModel(GraphBlock graphBlock)
        {
            this.Id = graphBlock.BlockId;
            this.Type = StoryBlockType.GRAPH;
            this.GraphId = graphBlock.GraphId;
            this.BlockPosition = graphBlock.BlockPosition;
        }

        public StoryBlockModel(TextBlock textBlock)
        {
            this.Id = textBlock.BlockId;
            this.Type = StoryBlockType.TEXTDB;
            this.EditorState = textBlock.EditorState;
            this.BlockPosition = textBlock.BlockPosition;
        }

        public int CompareTo(StoryBlockModel other)
        {
            return this.BlockPosition.CompareTo(other.BlockPosition); //TODO: add unit test testing sort is ascending
        }
    }
}
