﻿namespace HourglassServer.Data.DataManipulation.StoryOperations
{
    using System;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Models.Persistent;

    public class StoryFactory
    {
        public static Story CreateStoryFromStoryModel(StoryApplicationModel model)
        {
            Story newStory = new Story();

            newStory.StoryId = model.Id;
            newStory.UserId = model.UserId;
            newStory.PublicationStatus = model.PublicationStatus.ToString();
            newStory.Title = model.Title;
            newStory.Description = model.Description;

            return newStory;
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
        
        public static ImageBlock CreateImageBlockFromStoryBlockModel(StoryBlockModel model, string storyId)
        {
            ImageBlock newImageBlock = new ImageBlock
            {
                StoryId = storyId,
                BlockPosition = model.BlockPosition,
                BlockId = model.Id,
                ImageUrl = model.ImageUrl,
            };
            return newImageBlock;
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

        public static PublicationStatus GetPublicationStatus(Story story)
        {
            return (PublicationStatus) Enum.Parse(typeof(PublicationStatus), story.PublicationStatus);
        }

        public static bool StoryIsInStatus(Story story, PublicationStatus expectedStatus)
        {
            PublicationStatus actualPublicationStatus = GetPublicationStatus(story);
            return expectedStatus == actualPublicationStatus; 
        }

        public static DateTime GetNow()
        {
            return DateTime.UtcNow;
        }
    }
}
