using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HourglassServer.Data.DataManipulation.StoryOperations;
using System;

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
        public Mock<DbSet<ImageBlock>> MockImageBlockDbSet;
        public Mock<DbSet<Person>> MockPersonDbSet;

        public readonly string EditorState;
        public readonly string UserId;
        public readonly string UserName;
        public readonly string StoryDescription;
        public readonly string StoryTitle;
        public readonly DateTime DateCreated;
        public readonly DateTime DateLastEdited;
        public readonly string ImageUrl;

        public StoryTestData(): base()
        {
            StoryId = CreateUUID();
            GraphBlockId = CreateUUID();
            TextBlockId = CreateUUID();
            GeoMapBlockId = CreateUUID();

            EditorState = "{\"MeaningOfLife\": 42}";
            UserId = "test@test.com";
            UserName = "Test User";
            StoryDescription = "Sample Description";
            StoryTitle = "Example Title";
            DateCreated = StoryFactory.GetNow();
            DateLastEdited = StoryFactory.GetNow();
            ImageUrl = "https://images2.minutemediacdn.com/image/upload/c_fill,g_auto,h_1248,w_2220/f_auto,q_auto,w_1100/v1555924299/shape/mentalfloss/rick_astley.jpg";

            CreateEmptyMockDbSets();
            AddStoryApplicationModelToMockContext();
        }

        protected override void CreateEmptyMockDbSets()
        {
            MockStoryDbSet = new Mock<DbSet<Story>>();
            MockTextBlockDbSet = new Mock<DbSet<TextBlock>>();
            MockGraphBlockDbSet = new Mock<DbSet<GraphBlock>>();
            MockGeoMapBlockDbDSet = new Mock<DbSet<GeoMapBlock>>();
            MockImageBlockDbSet = new Mock<DbSet<ImageBlock>>();
            MockPersonDbSet = new Mock<DbSet<Person>>();

            CreateQueryableMockDbSet(MockStoryDbSet, new List<Story>());
            CreateQueryableMockDbSet(MockTextBlockDbSet, new List<TextBlock>());
            CreateQueryableMockDbSet(MockGraphBlockDbSet, new List<GraphBlock>());
            CreateQueryableMockDbSet(MockGeoMapBlockDbDSet, new List<GeoMapBlock>());
            CreateQueryableMockDbSet(MockImageBlockDbSet, new List<ImageBlock>());
            CreateQueryableMockDbSet(MockPersonDbSet, new List<Person>());
        }

        protected override void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.Story).Returns(MockStoryDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(MockTextBlockDbSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(MockGraphBlockDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(MockGeoMapBlockDbDSet.Object);
            MockContext.Setup(m => m.ImageBlock).Returns(MockImageBlockDbSet.Object);
            MockContext.Setup(m => m.Person).Returns(MockPersonDbSet.Object);
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
            CreateQueryableMockSetWithItem(MockImageBlockDbSet, CreateImageBlock());
            CreateQueryableMockSetWithItem(MockPersonDbSet, CreatePerson());

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
                PublicationStatus = PublicationStatus.DRAFT.ToString(),
                DateCreated = DateCreated,
                DateLastEdited = DateLastEdited
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

        private Person CreatePerson()
        {
            return new Person
            {
                Email = UserId,
                Name = UserName
            };
        }
    }
}
