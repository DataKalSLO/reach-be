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
        public string id { get; set; }

        [Required]
        public string type { get; set; }

        public string graphID { get; set; }

        public string mapID { get; set; }

        public string editorState { get; set; }

        /* Note, FEND will not define this and will be null after initial model binding
         * This means that this is required to be set based on its position in
         * given Story's storyBlocks
         */
        public int blockPosition { get; set; }

        public StoryBlockModel() { }

        public StoryBlockModel(GeoMapBlock mapBlock, int position)
        {
            this.id = mapBlock.BlockId;
            this.type = "GEOMAP"; //TODO: Find design pattern to make type extendable
            this.mapID = mapBlock.GeoMapId;
            this.blockPosition = position;
        }

        public StoryBlockModel(GraphBlock graphBlock, int position)
        {
            this.id = graphBlock.BlockId;
            this.type = "GRAPH";
            this.graphID = graphBlock.GraphId;
            this.blockPosition = position;
        }

        public StoryBlockModel(TextBlock textBlock, int position)
        {
            this.id = textBlock.BlockId;
            this.type = "TEXTDB";
            this.editorState = textBlock.EditorState;
            this.blockPosition = position;
        }


        public int CompareTo(StoryBlockModel other)
        {
            if (this.blockPosition < other.blockPosition)
                return -1;
            else if (this.blockPosition > other.blockPosition)
                return 1;
            else
                return 0;
        }
    }
}
