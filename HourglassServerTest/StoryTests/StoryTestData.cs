using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    public class StoryTestData : TestData
    {
        public string StoryId { get; }
        public string GraphId { get; }
        public string GeoMapId { get; }
        public string GraphBlockId { get; }
        public string TextBlockId { get; }
        public string GeoMapBlockId = CreateUUID();

        public GeoMapBlock GeoMapBlock; //TODO: make getters and change to private
        public GraphBlock GraphBlock;
        public TextBlock TextBlock;
        public Story Story;
        public List<StoryBlock> StoryBlocks;

        private Mock<DbSet<Story>> StoryDbSet;
        private Mock<DbSet<TextBlock>> TextBlockDbSet;
        private Mock<DbSet<GraphBlock>> GraphBlockDbSet;
        private Mock<DbSet<GeoMapBlock>> GeoMapBlockDbDSet;
        private Mock<DbSet<StoryBlock>> StoryBlockDbSet;

        private const string EditorState = "{\"MeaningOfLife\": 42}";
        private const string UserId = "test@test.com";
        private const string StoryDescription = "Sample Description";
        private const string StoryTitle = "Example Title";


        public StoryTestData()
        {
            this.StoryId = CreateUUID();
            this.GraphId = CreateUUID();
            this.GeoMapId = CreateUUID();
            this.GraphBlockId = CreateUUID();
            this.TextBlockId = CreateUUID();
            this.GeoMapBlockId = CreateUUID();
            InitializeMockSets();
            AddItemToMockContext();
        }

        override
        public void AddItemToMockContext()
        {
            this.GeoMapBlock = new GeoMapBlock
            {
                BlockId = GeoMapBlockId,
                GeoMapId = CreateUUID()
            };
            this.GraphBlock = new GraphBlock
            {
                BlockId = GraphBlockId,
                GraphId = GraphId
            };
            this.TextBlock = new TextBlock
            {
                BlockId = TextBlockId,
                EditorState = EditorState
            };
            this.Story = new Story
            {
                StoryId = StoryId,
                Title = StoryTitle,
                Description = StoryDescription,
                UserId = UserId,
                PublicationStatus = PublicationStatus.DRAFT.ToString()
            };
            Assert.IsNotNull(TextBlock, "failed to initialize textblock");
            Assert.IsNotNull(GeoMapBlock, "failed to initialize GeoMapBlock");

            this.StoryBlocks = new List<StoryBlock>
            {
                StoryFactory.CreateStoryBlockFromStoryBlockModel(new StoryBlockModel(TextBlock, 0), StoryId),
                StoryFactory.CreateStoryBlockFromStoryBlockModel(new StoryBlockModel(GeoMapBlock, 1), StoryId),
                StoryFactory.CreateStoryBlockFromStoryBlockModel(new StoryBlockModel(GraphBlock, 2), StoryId)
            };
            CreateQueryableMockSetWithItem(StoryDbSet, Story);
            CreateQueryableMockSetWithItem(TextBlockDbSet, TextBlock);
            CreateQueryableMockSetWithItem(GraphBlockDbSet, GraphBlock);
            CreateQueryableMockSetWithItem(GeoMapBlockDbDSet, GeoMapBlock);
            CreateQueryableMockDbSet(StoryBlockDbSet, StoryBlocks);
            AddDbSetsToMockContext();
        }

        override
        public void InitializeMockSets()
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
            AddDbSetsToMockContext();
        }

        override
        protected void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.Story).Returns(StoryDbSet.Object);
            MockContext.Setup(m => m.TextBlock).Returns(TextBlockDbSet.Object);
            MockContext.Setup(m => m.GraphBlock).Returns(GraphBlockDbSet.Object);
            MockContext.Setup(m => m.GeoMapBlock).Returns(GeoMapBlockDbDSet.Object);
            MockContext.Setup(m => m.StoryBlock).Returns(StoryBlockDbSet.Object);
        }
    }
}
