using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryOperations;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


/* This class is instantiated with the following entities:
 * 
 * - Story + (associated) TextBlock, GraphBlock, GeoMapBlock
 */
namespace HourglassServerTest.StoryTests
{
    public class StoryTestData: TestData
    {
        public string StoryId { get; }
        public string GraphBlockId { get; }
        public string TextBlockId { get; }
        public string GeoMapBlockId { get; }

        public Mock<DbSet<Story>> MockStoryDbSet;
        public Mock<DbSet<TextBlock>> MockTextBlockDbSet;
        public Mock<DbSet<GraphBlock>> MockGraphBlockDbSet;
        public Mock<DbSet<GeoMapBlock>> MockGeoMapBlockDbDSet;

        public readonly string EditorState;
        public readonly string UserId;
        public readonly string StoryDescription;
        public readonly string StoryTitle;

        public StoryTestData(): base()
        {
            StoryId = CreateUUID();
            GraphBlockId = CreateUUID();
            TextBlockId = CreateUUID();
            GeoMapBlockId = CreateUUID();

            EditorState = "{\"MeaningOfLife\": 42}";
            UserId = "test@test.com";
            StoryDescription = "Sample Description";
            StoryTitle = "Example Title";

            CreateEmptyMockDbSets();
            AddStoryApplicationModelToMockContext();
        }

        /*
         * Required Abstract Methods
         */

        protected override void CreateEmptyMockDbSets()
        {
            MockStoryDbSet = new Mock<DbSet<Story>>();
            MockTextBlockDbSet = new Mock<DbSet<TextBlock>>();
            MockGraphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            MockGeoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();

            CreateQueryableMockDbSet(MockStoryDbSet, new List<Story>());
            CreateQueryableMockDbSet(MockTextBlockDbSet, new List<TextBlock>());
            CreateQueryableMockDbSet(MockGraphBlockDbSet, new List<GraphBlock>());
            CreateQueryableMockDbSet(MockGeoMapBlockDbDSet, new List<GeoMapBlock>());
        }

        protected override void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.Story).Returns(MockStoryDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(MockTextBlockDbSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(MockGraphBlockDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(MockGeoMapBlockDbDSet.Object);
        }

        /*
         * Content Creation Methods
         */

        private void AddStoryApplicationModelToMockContext()
        {
            CreateQueryableMockSetWithItem(MockStoryDbSet, CreateStory());
            CreateQueryableMockSetWithItem(MockTextBlockDbSet, CreateTextBlock());
            CreateQueryableMockSetWithItem(MockGraphBlockDbSet, CreateGraphBlock());
            CreateQueryableMockSetWithItem(MockGeoMapBlockDbDSet, CreateGeoMapBlock());
            AddDbSetsToMockContext();
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
                StoryId = StoryId,
                BlockPosition = 0,
                BlockId = this.TextBlockId,
                EditorState = EditorState
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

        private GeoMapBlock CreateGeoMapBlock()
        {
            return new GeoMapBlock
            {
                StoryId = StoryId,
                BlockPosition = 2,
                BlockId = GeoMapBlockId,
                GeoMapId = CreateUUID()
            };
        }
    }
}
