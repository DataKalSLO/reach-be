namespace HourglassServer.Data.DataManipulation.StoryModel
{
    using System;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Models.Persistent;

    public class StoryFactory
    {
        public static Story CreateStoryFromStoryModel(StoryApplicationModel model)
        {
            Story newStory = new Story();
            DateTime nowTimeStamp = DateTime.UtcNow;

            newStory.StoryId = model.Id;
            newStory.UserId = model.UserId;
            newStory.PublicationStatus = PublicationStatus.DRAFT.ToString();
            newStory.Title = model.Title;
            newStory.Description = model.Description;
            newStory.DateCreated = nowTimeStamp;
            newStory.DateLastEdited = nowTimeStamp;

            return newStory; 
        }

        public static StoryBlockModel CreateStoryBlockModel(TextBlock textBlock)
        {
            return new StoryBlockModel()
            {
                Id = textBlock.BlockId,
                Type = StoryBlockType.TEXTDB,
                EditorState = textBlock.EditorState,
            };
        }

        public static StoryBlockModel CreateStoryBlockModel(GraphBlock graphBlock)
        {
            return new StoryBlockModel()
            {
                Id = graphBlock.BlockId,
                Type = StoryBlockType.TEXTDB,
                GraphId = graphBlock.GraphId,
            };
        }

        public static StoryBlockModel CreateStoryBlockModel(GeoMapBlock geoMapBlock)
        {
            return new StoryBlockModel()
            {
                Id = geoMapBlock.BlockId,
                Type = StoryBlockType.TEXTDB,
                MapId = geoMapBlock.GeoMapId,
            };
        }

        public static TextBlock CreateTextBlockFromStoryBlockModel(StoryBlockModel model, string storyId)
        {
            TextBlock newTextBlock = new TextBlock
            {
                StoryId = storyId,
                BlockPosition = model.BlockPosition,
                BlockId = model.Id,
                EditorState = model.EditorState,
            };
            return newTextBlock;
        }

        public static GraphBlock CreateGraphBlockFromStoryBlockModel(StoryBlockModel model, string storyId)
        {
            GraphBlock newGraphBlock = new GraphBlock
            {
                StoryId = storyId,
                BlockPosition = model.BlockPosition,
                BlockId = model.Id,
                GraphId = model.GraphId,
            };
            return newGraphBlock;
        }

        public static GeoMapBlock CreateGeoMapBlockFromStoryBlockModel(StoryBlockModel model, string storyId)
        {
            GeoMapBlock newTextBlock = new GeoMapBlock
            {
                StoryId = storyId,
                BlockPosition = model.BlockPosition,
                BlockId = model.Id,
                GeoMapId = model.MapId,
            };
            return newTextBlock;
        }

        public static GraphBlock CreateGraphBlock(string storyId, int blockPosition, string blockId, string graphId)
        {
            GraphBlock graphBlock = new GraphBlock
            {
                StoryId = storyId,
                BlockPosition = blockPosition,
                BlockId = blockId,
                GraphId = graphId,
            };
            return graphBlock;
        }

        public static TextBlock CreateTextBlock(string storyId, int blockPosition, string blockId, string editorState)
        {
            TextBlock textBlock = new TextBlock
            {
                StoryId = storyId,
                BlockPosition = blockPosition,
                BlockId = blockId,
                EditorState = editorState,
            };
            return textBlock;
        }

        public static GeoMapBlock CreateGeoMapBlock(string storyId, int blockPosition, string blockId, string geoMapId)
        {
            GeoMapBlock geoMapBlock = new GeoMapBlock
            {
                StoryId = storyId,
                BlockPosition = blockPosition,
                BlockId = blockId,
                GeoMapId = geoMapId,
            };
            return geoMapBlock;
        }
    }
}
