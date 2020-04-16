using System;
using Moq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.Persistent;
using Microsoft.EntityFrameworkCore;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;

namespace HourglassServerTest.StoryModel
{
    public class StoryTestData
    {
        public Mock<DbSet<Story>> storyDbSet ;
        public Mock<DbSet<TextBlock>> textBlockDbSet ;
        public Mock<DbSet<GraphBlock>> graphBlockDbSet;
        public Mock<DbSet<GeoMapBlock>> geoMapBlockDbDSet;
        public Mock<DbSet<StoryBlock>> storyBlockDbSet;

     
        public void StoryTestData()
        {
            storyDbSet = new Mock<DbSet<Story>>();
            textBlockDbSet = new Mock<DbSet<TextBlock>>();
            graphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            geoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();
            storyBlockDbSet = new Mock<DbSet<StoryBlock>>();
        }

        public postgresContext GetMockContext()
        {
            var mockContext = new Mock<postgresContext>();
            mockContext.Setup(m => m.Story).Returns(storyDbSet.Object);
            mockContext.Setup(m => m.TextBlock).Returns(textBlockDbSet.Object);
            mockContext.Setup(m => m.GraphBlock).Returns(graphBlockDbSet.Object);
            mockContext.Setup(m => m.GeoMapBlock).Returns(geoMapBlockDbDSet.Object);
            mockContext.Setup(m => m.StoryBlock).Returns(storyBlockDbSet.Object);
            return mockContext.Object;
        }

        public static StoryApplicationModel GetValidStory()
        {
            StoryApplicationModel storyModel = new StoryApplicationModel();
            storyModel.Id = GetNewId().ToString();
            storyModel.StoryBlocks = GetStoryBlocks();
            return storyModel;
        }

        public static List<StoryBlockModel> GetStoryBlocks()
        {
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            storyBlocks.Add(new StoryBlockModel(GetNewTextBlock(), 0));
            storyBlocks.Add(new StoryBlockModel(GetNewGeoMapBlock(), 1));
            storyBlocks.Add(new StoryBlockModel(GetNewGraphBlock(), 2));
            return storyBlocks;
        }

        public static TextBlock GetNewTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.BlockId = GetNewId().ToString();
            textBlock.EditorState = "{\"MeaningOfLife\": 42}";
            return textBlock;
        }

        public static GraphBlock GetNewGraphBlock()
        {
            GraphBlock textBlock = new GraphBlock();
            textBlock.BlockId = GetNewId();
            textBlock.GraphId = GetNewId();
            return textBlock;
        }

        public static GeoMapBlock GetNewGeoMapBlock()
        {
            GeoMapBlock geoMapBlock = new GeoMapBlock();
            geoMapBlock.BlockId = GetNewId();
            geoMapBlock.GeoMapId = GetNewId();
            return geoMapBlock;
        }

        public static string GetNewId()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
