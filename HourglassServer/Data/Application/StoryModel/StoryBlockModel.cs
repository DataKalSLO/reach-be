using System;
using System.ComponentModel.DataAnnotations;
using HourglassServer.Data.Persistent;
using System.Collections.Generic;


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
        public string type { get; set; }

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
            this.type = "GEOMAP"; //TODO: Find design pattern to make type extendable
            this.Id = mapBlock.BlockId;
            this.MapId = mapBlock.GeoMapId;
            this.BlockPosition = position;
        }

        public StoryBlockModel(GraphBlock graphBlock, int position)
        {
            this.type = "GRAPH";
            this.Id = graphBlock.BlockId;
            this.GraphId = graphBlock.GraphId;
            this.BlockPosition = position;
        }

        public StoryBlockModel(TextBlock textBlock, int position)
        {
            this.type = "TEXTDB";
            this.Id = textBlock.BlockId;
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
