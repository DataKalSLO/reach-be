using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
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

        //TODO: Prefix these variables with `Mock`
        public readonly Mock<HourglassContext> MockContext;
        public Mock<DbSet<Story>> StoryDbSet;
        public Mock<DbSet<TextBlock>> TextBlockDbSet;
        public Mock<DbSet<GraphBlock>> GraphBlockDbSet;
        public Mock<DbSet<GeoMapBlock>> GeoMapBlockDbDSet;
        public Mock<DbSet<StoryBlock>> StoryBlockDbSet;

        private const string EditorState = "{\"MeaningOfLife\": 42}";
        private const string UserId = "test@test.com";
        private const string StoryDescription = "Sample Description";
        private const string StoryTitle = "Example Title";

        public StoryTestData()
        {
            this.StoryId = CreateUUID();
            this.GraphBlockId = CreateUUID();
            this.TextBlockId = CreateUUID();
            this.GeoMapBlockId = CreateUUID();
            MockContext = new Mock<HourglassContext>();
            CreateEmptyMockDbSets();
            AddStoryApplicationModelToMockContext();
        }

        public HourglassContext GetMockContext()
        {
            return MockContext.Object;
        }

        public void ClearDataInContext()
        {
            CreateEmptyMockDbSets();
            AddDbSetsToMockContext();
        }

        private void AddStoryApplicationModelToMockContext()
        {
            CreateQueryableMockSetWithItem(StoryDbSet, CreateStory());
            CreateQueryableMockSetWithItem(TextBlockDbSet, CreateTextBlock());
            CreateQueryableMockSetWithItem(GraphBlockDbSet, CreateGraphBlock());
            CreateQueryableMockSetWithItem(GeoMapBlockDbDSet, CreateGeoMapBlock());
            CreateQueryableMockDbSet(StoryBlockDbSet, CreateListOfStoryBlocks());
            AddDbSetsToMockContext();
        }

        private void CreateEmptyMockDbSets()
        {
            StoryDbSet = new Mock<DbSet<Story>>();
            TextBlockDbSet = new Mock<DbSet<TextBlock>>();
            GraphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            GeoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();
            StoryBlockDbSet = new Mock<DbSet<StoryBlock>>();

            CreateQueryableMockDbSet(StoryDbSet, new List<Story>());
            CreateQueryableMockDbSet(TextBlockDbSet, new List<TextBlock>());
            CreateQueryableMockDbSet(GraphBlockDbSet, new List<GraphBlock>());
            CreateQueryableMockDbSet(GeoMapBlockDbDSet, new List<GeoMapBlock>());
            CreateQueryableMockDbSet(StoryBlockDbSet, new List<StoryBlock>());
        }

        private void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.Story).Returns(StoryDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(TextBlockDbSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(GraphBlockDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(GeoMapBlockDbDSet.Object);
            MockContext.Setup(m => m.StoryBlock).Returns(StoryBlockDbSet.Object);
        }

        private Mock<DbSet<T>> CreateQueryableMockSetWithItem<T>(Mock<DbSet<T>> mockSet,T item) where T : class
        {
            List<T> items = new List<T>() { item };
            return CreateQueryableMockDbSet(mockSet, items);
        }

        private Mock<DbSet<T>> CreateQueryableMockDbSet <T> (Mock<DbSet<T>> mockSet, List<T> sourceList) where T : class
        {
            IQueryable<T> queryableList = sourceList.AsQueryable();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            //TODO: Find a way to moq updating items in list.
            return mockSet;
        }

        private List<StoryBlock> CreateListOfStoryBlocks()
        {
            List<StoryBlockModel> storyBlockModels= CreateListOfStoryBlockModels();
            List<StoryBlock> storyBlocks = new List<StoryBlock>
            {
                StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[0], StoryId),
                StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[1], StoryId),
                StoryFactory.CreateStoryBlockFromStoryBlockModel(storyBlockModels[2], StoryId)
            };
            return storyBlocks;
        }

        private List<StoryBlockModel> CreateListOfStoryBlockModels()
        {
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            storyBlocks.Add(new StoryBlockModel(CreateTextBlock(), 0));
            storyBlocks.Add(new StoryBlockModel(CreateGeoMapBlock(), 1));
            storyBlocks.Add(new StoryBlockModel(CreateGraphBlock(), 2));
            return storyBlocks;
        }

        private Story CreateStory()
        {
            return new Story
            {
                StoryId = this.StoryId,
                Title = StoryTitle,
                Description = StoryDescription,
                UserId = UserId,
                PublicationStatus = PublicationStatus.DRAFT.ToString()
            };
        }

        private TextBlock CreateTextBlock()
        {
            return new TextBlock
            {
                BlockId = this.TextBlockId,
                EditorState = EditorState
            };
        }

        private GraphBlock CreateGraphBlock()
        {
            return new GraphBlock
            {
                BlockId = GraphBlockId,
                GraphId = CreateUUID()
            };
        }

        private GeoMapBlock CreateGeoMapBlock()
        {
            return new GeoMapBlock
            {
                BlockId = GeoMapBlockId,
                GeoMapId = CreateUUID()
            };
        }

        private static string CreateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
