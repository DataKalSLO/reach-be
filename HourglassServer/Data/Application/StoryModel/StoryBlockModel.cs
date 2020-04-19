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

        
        [Required]  //Check only performed during model binding and defaults to GRAPH
        [JsonConverter(typeof(StringEnumConverter))] 
        public StoryBlockType Type { get; set; } 

        public string GraphId { get; set; }

        public string MapId { get; set; }

        public string EditorState { get; set; }

        public int BlockPosition { get; set; }

        public StoryBlockModel() {}

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
            return this.BlockPosition.CompareTo(other.BlockPosition); //TODO: add unit test testing sort is ascending
        }
    }
}
