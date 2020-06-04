using Moq; 
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


/* This class is instantiated with the following entities:
 *
 * - BookmarkGeoMap
 * - BookmarkGraph
 * - BookmarkStory
 */
namespace HourglassServerTest.StoryTests
{
    public class BookmarkTestData: TestData
    {
        public readonly string UserId;
        public readonly string GeoMapId;
        public readonly string GraphId;
        public readonly string StoryId;

        public Mock<DbSet<BookmarkGeoMap>> MockBookmarkGeoMapDbSet;
        public Mock<DbSet<BookmarkGraph>> MockBookmarkGraphDbSet;
        public Mock<DbSet<BookmarkStory>> MockBookmarkStoryDbDSet;

        public BookmarkGeoMap bookmarkGeoMap;
        public BookmarkGraph bookmarkGraph;
        public BookmarkStory bookmarkStory;

        public BookmarkTestData(): base()
        {
            UserId = "test@test.com";
            GeoMapId = CreateUUID();
            GraphId = CreateUUID();
            StoryId = CreateUUID();

            SetBookmarkGeoMap();
            SetBookmarkGraph();
            SetBookmarkStory();

            AddBookmarksToMockContext();
        }
        
        /*
         * Required Abstract Methods
         */

        protected override void CreateEmptyMockDbSets()
        {
            MockBookmarkGeoMapDbSet = new Mock<DbSet<BookmarkGeoMap>>();
            MockBookmarkGraphDbSet = new Mock<DbSet<BookmarkGraph>>();
            MockBookmarkStoryDbDSet = new Mock<DbSet<BookmarkStory>>();

            CreateQueryableMockDbSet(MockBookmarkGeoMapDbSet, new List<BookmarkGeoMap>());
            CreateQueryableMockDbSet(MockBookmarkGraphDbSet, new List<BookmarkGraph>());
            CreateQueryableMockDbSet(MockBookmarkStoryDbDSet, new List<BookmarkStory>());
        }

        protected override void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.BookmarkGeoMap).Returns(MockBookmarkGeoMapDbSet.Object);
            MockContext.Setup(m => m.BookmarkGraph).Returns(MockBookmarkGraphDbSet.Object);
            MockContext.Setup(m => m.BookmarkStory).Returns(MockBookmarkStoryDbDSet.Object);
        }

        /*
         * Content Creation Methods
         */

        private void AddBookmarksToMockContext()
        {
            CreateQueryableMockSetWithItem(MockBookmarkGeoMapDbSet, bookmarkGeoMap);
            CreateQueryableMockSetWithItem(MockBookmarkGraphDbSet, bookmarkGraph);
            CreateQueryableMockSetWithItem(MockBookmarkStoryDbDSet, bookmarkStory);
            AddDbSetsToMockContext();
        }

        public void SetBookmarkGeoMap()
        {
            bookmarkGeoMap = new BookmarkGeoMap
            {
                UserId = UserId,
                GeoMapId = GeoMapId
            };
        }

        public void SetBookmarkGraph()
        {
            bookmarkGraph = new BookmarkGraph
            {
                UserId = UserId,
                GraphId = GraphId
            };
        }

        public void SetBookmarkStory()
        {
            bookmarkStory = new BookmarkStory
            {
                UserId = UserId,
                StoryId = StoryId
            };
        }
    }
}
