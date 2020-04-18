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
            TextBlock newTextBlock = new TextBlock();
            newTextBlock.BlockId = model.Id;
            newTextBlock.EditorState = model.EditorState;
            return newTextBlock;
        }

        public static GraphBlock CreateGraphBlockFromStoryBlockModel(StoryBlockModel model)
        {
            GraphBlock newGraphBlock = new GraphBlock();
            newGraphBlock.BlockId = model.Id;
            newGraphBlock.GraphId = model.GraphId;
            return newGraphBlock;
        }

        public static GeoMapBlock CreateGeoMapBlockFromStoryBlockModel(StoryBlockModel model)
        {
            GeoMapBlock newTextBlock = new GeoMapBlock();
            newTextBlock.BlockId = model.Id;
            newTextBlock.GeoMapId = model.MapId;
            return newTextBlock;
        }

        public static StoryBlock CreateStoryBlockFromStoryBlockModel(StoryBlockModel model, string StoryId)
        {
            StoryBlock newStoryBlock = new StoryBlock();
            newStoryBlock.BlockId = model.Id;
            newStoryBlock.StoryId = StoryId;
            newStoryBlock.BlockPosition = model.BlockPosition;
            return newStoryBlock;
        }

        public static GraphBlock createGraphBlock(string BlockId, string GraphId)
        {
            GraphBlock graphBlock = new GraphBlock();
            graphBlock.BlockId = BlockId;
            graphBlock.GraphId = GraphId;
            return graphBlock;
        }

        public static TextBlock createTextBlock(string BlockId, string EditorState)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.BlockId = BlockId;
            textBlock.EditorState = EditorState;
            return textBlock;
        }

        public static GeoMapBlock createGeoMapBlock(string BlockId, string GeoMapId)
        {
            GeoMapBlock geoMapBlock = new GeoMapBlock();
            geoMapBlock.BlockId = BlockId;
            geoMapBlock.GeoMapId = GeoMapId;
            return geoMapBlock;
        }
    }
}
