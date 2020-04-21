using Moq; 
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using HourglassServerTest.StoryTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

/* This files contains a context with the following items
 * 1 Story bookmark, 1 graph bookmark, and 1 map bookmark
 */
 //TODO: 
namespace HourglassServerTest.BookmarkTests
{
    public class BookmarkTestData : TestData
    {
        public string UserId { get; }

        private StoryTestData storyTestData;

        private Mock<DbSet<StoryBookmark>> StoryBookmarkDbSet;
        private Mock<DbSet<GraphBookmark>> GraphBookmarkDbSet;
        private Mock<DbSet<GeoMapBookmark>> GeoMapBookmarkDbSet;
        private Mock<DbSet<Graph>> GraphDbSet; //TODO: Create a GraphTestData
        private Mock<DbSet<GeoMap>> GeoMapDbSet; //TODO: Create GeoMapTestData

        public string StoryId;

        public BookmarkTestData() : base()
        {
            this.UserId = CreateUUID();
            this.storyTestData = new StoryTestData();
            this.StoryId = storyTestData.StoryId;
            InitializeMockSets();
            AddItemToMockContext();
        }

        override
        public void AddItemToMockContext()
        {
            this.storyTestData.AddItemToMockContext();
            //this.graphTestData.AddItemToMockContent(); TODO: Replace with line below when graphTestData exists
            CreateQueryableMockSetWithItem(GraphDbSet, GetGraph());

            //this.geoMapTestData.AddItemToMockContent(); TODO: Replace with line below when geoMapTestData exists
            CreateQueryableMockSetWithItem(GeoMapDbSet, GetGeoMap());

            CreateQueryableMockSetWithItem(StoryBookmarkDbSet, GetStoryBookmark());
            CreateQueryableMockSetWithItem(GraphBookmarkDbSet, GetGraphBookmark());
            CreateQueryableMockSetWithItem(GeoMapBookmarkDbSet, GetGeoMapBookmark());
            
            AddDbSetsToMockContext();
        }

        override
        public void InitializeMockSets()
        {
            this.storyTestData.InitializeMockSets();
            StoryBookmarkDbSet = new Mock<DbSet<StoryBookmark>>();
            GraphBookmarkDbSet = new Mock<DbSet<GraphBookmark>>();
            GeoMapBookmarkDbSet = new Mock<DbSet<GeoMapBookmark>>();
            GraphDbSet = new Mock<DbSet<Graph>>(); //TODO: Delete after GraphTesteData implemented
            GeoMapDbSet = new Mock<DbSet<GeoMap>>(); //TODO: Delete after GeoMapTestData implemented

            CreateQueryableMockDbSet(StoryBookmarkDbSet, new List<StoryBookmark>());
            CreateQueryableMockDbSet(GraphBookmarkDbSet, new List<GraphBookmark>());
            CreateQueryableMockDbSet(GeoMapBookmarkDbSet, new List<GeoMapBookmark>());
            AddDbSetsToMockContext();
        }

        override
        protected void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.StoryBookmark).Returns(StoryBookmarkDbSet.Object);
            MockContext.Setup(m => m.GraphBookmark).Returns(GraphBookmarkDbSet.Object);
            MockContext.Setup(m => m.GeoMapBookmark).Returns(GeoMapBookmarkDbSet.Object);

            MockContext.Setup(m => m.Graph).Returns(GraphDbSet.Object); //TODO: Remove statement after Graph + Map Test Data
            MockContext.Setup(m => m.GeoMap).Returns(GeoMapDbSet.Object); //TODO: Remove statements after Graph + Map Test Data
        }

        public StoryBookmark GetStoryBookmark()
        {
            return new StoryBookmark
            {
                StoryId = this.storyTestData.StoryId,
                UserId = this.UserId
            };
        }

        public GraphBookmark GetGraphBookmark()
        {
            return new GraphBookmark
            {
                UserId = UserId,
                GraphId = CreateUUID() //TODO: Replace with GraphTestData when implemented
            };
        }

        public GeoMapBookmark GetGeoMapBookmark()
        {
            return new GeoMapBookmark
            {
                UserId = UserId,
                GeoMapId = CreateUUID() //TODO: Replace with GeoMapTestData when implemented
            };
        }

        public Graph GetGraph()
        {
            return new Graph
            {
                //UserId = UserId, TODO: Uncomment once `graph` table has column `user_id`
                GraphId = CreateUUID() //TODO: Replace with GeoMapTestData when implemented
            };
        }

        public GeoMap GetGeoMap()
        {
            return new GeoMap
            {
                //UserId = UserId, TODO: Uncomment once `graph` table has column `user_id`
                GeoMapId = CreateUUID() //TODO: Replace with GeoMapTestData when implemented
            };
        }
    }
}
