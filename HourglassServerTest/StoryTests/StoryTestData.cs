using System;
using Moq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Data.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    public class StoryTestData
    {
        public string StoryId { get; }
        public string GraphBlockId { get; }
        public string TextBlockId { get; }
        public string GeoMapBlockId { get; }

        public string StoryTitle { get;  }
        public string StoryDescription { get; }
        public string UserId { get;  }
        public string EditorState { get; }


        private readonly Mock<postgresContext> MockContext;
        private readonly Mock<DbSet<Story>> StoryDbSet;
        private readonly Mock<DbSet<TextBlock>> TextBlockDbSet;
        private readonly Mock<DbSet<GraphBlock>> GraphBlockDbSet;
        private readonly Mock<DbSet<GeoMapBlock>> GeoMapBlockDbDSet;
        private readonly Mock<DbSet<StoryBlock>> StoryBlockDbSet;

        public StoryTestData()
        {
            this.StoryId = GetNewId();
            this.GraphBlockId = GetNewId();
            this.TextBlockId = GetNewId();
            this.GeoMapBlockId = GetNewId();
            this.StoryTitle = "Example Title";
            this.StoryDescription = "Sample Description";
            this.UserId = "test@test.com";
            this.EditorState = "{\"MeaningOfLife\": 42}";
            StoryDbSet = new Mock<DbSet<Story>>();
            TextBlockDbSet = new Mock<DbSet<TextBlock>>();
            GraphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            GeoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();
            StoryBlockDbSet = new Mock<DbSet<StoryBlock>>();
            MockContext = new Mock<postgresContext>();
            AddStoryApplicationModelToContext();
        }

        public postgresContext GetMockContext()
        {
            return MockContext.Object;
        }

        private void AddStoryApplicationModelToContext()
        {
            AddItemToMockSet(StoryDbSet, getNewStory());
            AddItemToMockSet(TextBlockDbSet, GetNewTextBlock());
            AddItemToMockSet(GraphBlockDbSet, GetNewGraphBlock());
            AddItemToMockSet(GeoMapBlockDbDSet, GetNewGeoMapBlock());
            AddItemsToMockSet(StoryBlockDbSet, GetStoryBlocks());
            AddDbSetsToContext();
        }

        private void AddDbSetsToContext()
        {
            MockContext.Setup(m => m.Story).Returns(StoryDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(TextBlockDbSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(GraphBlockDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(GeoMapBlockDbDSet.Object);
            MockContext.Setup(m => m.StoryBlock).Returns(StoryBlockDbSet.Object);
        }


        private Mock<DbSet<T>> AddItemToMockSet<T>(Mock<DbSet<T>> mockSet,T item) where T : class
        {
            List<T> items = new List<T>() { item };
            IQueryable<T> queryableList = items.AsQueryable();
            return AddQueryListToMockSet(mockSet, queryableList);
        }

        private Mock<DbSet<T>> AddItemsToMockSet<T>(Mock<DbSet<T>> mockSet, List<T> items) where T : class
        {
            IQueryable<T> queryableList = items.AsQueryable();
            return AddQueryListToMockSet(mockSet, queryableList);
        }

        private Mock<DbSet<T>> AddQueryListToMockSet <T> (Mock<DbSet<T>> mockSet, IQueryable<T> queryableList) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());
            return mockSet;
        }

        private List<StoryBlock> GetStoryBlocks()
        {
            List<StoryBlockModel> storyBlockModels= GetStoryBlockModels();
            List<StoryBlock> storyBlocks = new List<StoryBlock>();
            storyBlocks.Add(StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[0], StoryId));
            storyBlocks.Add(StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[1], StoryId));
            storyBlocks.Add(StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[2], StoryId));
            return storyBlocks;
        }

        private List<StoryBlockModel> GetStoryBlockModels()
        {
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            storyBlocks.Add(new StoryBlockModel(GetNewTextBlock(), 0));
            storyBlocks.Add(new StoryBlockModel(GetNewGeoMapBlock(), 1));
            storyBlocks.Add(new StoryBlockModel(GetNewGraphBlock(), 2));
            return storyBlocks;
        }

        private Story getNewStory()
        {
            return new Story
            {
                StoryId = this.StoryId,
                Title = StoryTitle,
                Description = StoryDescription,
                UserId = UserId,
                PublicationStatus = "DRAFT" //TODO: use enum and convert to string
            };
        }

        private TextBlock GetNewTextBlock()
        {
            return new TextBlock
            {
                BlockId = TextBlockId,
                EditorState = EditorState
            };
        }

        private GraphBlock GetNewGraphBlock()
        {
            return new GraphBlock
            {
                BlockId = GraphBlockId,
                GraphId = GetNewId()
            };
        }

        private GeoMapBlock GetNewGeoMapBlock()
        {
            return new GeoMapBlock
            {
                BlockId = GeoMapBlockId,
                GeoMapId = GetNewId()
            };
        }

        private static string GetNewId()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
