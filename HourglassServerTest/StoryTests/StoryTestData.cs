using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryOperations;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

/* Reminder to reader: This class is instantiated with the following entities
 * in the DbContext:
 *
 * - Story
 * - TextBlock + StoryBlock
 * - GraphBlock + StoryBlock
 * - GeoMapBlock + StoryBlock
 */
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
        public Mock<DbSet<GeoMapBlock>> GeoMapBlockDbDSet;
        public Mock<DbSet<ImageBlock>> ImageBlockDbSet;
        public Mock<DbSet<GraphBlock>> GraphBlockDbSet;
        public Mock<DbSet<TextBlock>> TextBlockDbSet;

        private const string EditorState = "{\"MeaningOfLife\": 42}";
        public readonly string UserId = "test@test.com";
        private const string StoryDescription = "Sample Description";
        private const string StoryTitle = "Example Title";
        public readonly string ImageUrl = "https://images2.minutemediacdn.com/image/upload/c_fill,g_auto,h_1248,w_2220/f_auto,q_auto,w_1100/v1555924299/shape/mentalfloss/rick_astley.jpg";

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
            CreateQueryableMockSetWithItem(ImageBlockDbSet, CreateImageBlock());
            AddDbSetsToMockContext();
        }

        private void CreateEmptyMockDbSets()
        {
            StoryDbSet = new Mock<DbSet<Story>>();
            GeoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();
            GraphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            ImageBlockDbSet = new Mock<DbSet<ImageBlock>>();
            TextBlockDbSet = new Mock<DbSet<TextBlock>>();

            CreateQueryableMockDbSet(StoryDbSet, new List<Story>());
            CreateQueryableMockDbSet(GeoMapBlockDbDSet, new List<GeoMapBlock>());
            CreateQueryableMockDbSet(GraphBlockDbSet, new List<GraphBlock>());
            CreateQueryableMockDbSet(ImageBlockDbSet, new List<ImageBlock>());
            CreateQueryableMockDbSet(TextBlockDbSet, new List<TextBlock>());
        }

        private void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.Story).Returns(StoryDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(GeoMapBlockDbDSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(GraphBlockDbSet.Object);
            MockContext.Setup(m => m.ImageBlock).Returns(ImageBlockDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(TextBlockDbSet.Object);
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
            //TODO: Find a way to mock both `Find` AND `Any` DbSet methods for testing updating stories.
            //TODO: Find a way to moq updating items in list.
            return mockSet;
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

        private GeoMapBlock CreateGeoMapBlock()
        {
            return new GeoMapBlock
            {
                StoryId = StoryId,
                BlockPosition = 0,
                BlockId = GeoMapBlockId,
                GeoMapId = CreateUUID()
            };
        }

        private GraphBlock CreateGraphBlock()
        {
            return new GraphBlock
            {
                StoryId = StoryId,
                BlockPosition = 1,
                BlockId = GraphBlockId,
                GraphId = CreateUUID()
            };
        }

        private ImageBlock CreateImageBlock()
        {
            return new ImageBlock
            {
                StoryId = StoryId,
                BlockPosition = 2,
                BlockId = GraphBlockId,
                ImageUrl = ImageUrl
            };
        }

        private TextBlock CreateTextBlock()
        {
            return new TextBlock
            {
                StoryId = StoryId,
                BlockPosition = 3,
                BlockId = this.TextBlockId,
                EditorState = EditorState
            };
        }

        private static string CreateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
