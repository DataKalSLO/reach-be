using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using System;

namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public class StoryFactory
    {
        public static Story CreateStoryFromStoryModel(StoryApplicationModel model)
        {
            Story newStory = new Story();
            DateTime nowTimeStamp = new DateTime();

            newStory.StoryId = model.Id;
            newStory.UserId = model.UserId;
            newStory.PublicationStatus = PublicationStatus.DRAFT.ToString();
            newStory.Title = model.Title;
            newStory.Description = model.Description;
            newStory.DateCreated = nowTimeStamp;
            newStory.DateLastEdited = nowTimeStamp;

            return newStory; 
        }

        public static TextBlock CreateTextBlockFromStoryBlockModel(StoryBlockModel model)
        {
            TextBlock newTextBlock = new TextBlock
            {
                BlockId = model.Id,
                EditorState = model.EditorState
            };
            return newTextBlock;
        }

        public static GraphBlock CreateGraphBlockFromStoryBlockModel(StoryBlockModel model)
        {
            GraphBlock newGraphBlock = new GraphBlock
            {
                BlockId = model.Id,
                GraphId = model.GraphId
            };
            return newGraphBlock;
        }

        public static GeoMapBlock CreateGeoMapBlockFromStoryBlockModel(StoryBlockModel model)
        {
            GeoMapBlock newTextBlock = new GeoMapBlock
            {
                BlockId = model.Id,
                GeoMapId = model.MapId
            };
            return newTextBlock;
        }

        public static StoryBlock CreateStoryBlockFromStoryBlockModel(StoryBlockModel model, string storyId)
        {
            StoryBlock newStoryBlock = new StoryBlock
            {
                BlockId = model.Id,
                StoryId = storyId,
                BlockPosition = model.BlockPosition
            };
            return newStoryBlock;
        }

        public static GraphBlock CreateGraphBlock(string blockId, string graphId)
        {
            GraphBlock graphBlock = new GraphBlock
            {
                BlockId = blockId,
                GraphId = graphId
            };
            return graphBlock;
        }

        public static TextBlock CreateTextBlock(string blockId, string editorState)
        {
            TextBlock textBlock = new TextBlock
            {
                BlockId = blockId,
                EditorState = editorState
            };
            return textBlock;
        }

        public static GeoMapBlock CreateGeoMapBlock(string blockId, string geoMapId)
        {
            GeoMapBlock geoMapBlock = new GeoMapBlock
            {
                BlockId = blockId,
                GeoMapId = geoMapId
            };
            return geoMapBlock;
        }
    }
}
