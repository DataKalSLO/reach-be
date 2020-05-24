using Moq; 
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
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
    public class BookmarkTestData
    {
        public readonly string UserId;
        public readonly string GeoMapId;
        public readonly string GraphId;
        public readonly string StoryId;

        public readonly Mock<HourglassContext> MockContext;

        public Mock<DbSet<BookmarkGeoMap>> MockBookmarkGeoMapDbSet;
        public Mock<DbSet<BookmarkGraph>> MockBookmarkGraphDbSet;
        public Mock<DbSet<BookmarkStory>> MockBookmarkStoryDbDSet;

        public BookmarkGeoMap bookmarkGeoMap;
        public BookmarkGraph bookmarkGraph;
        public BookmarkStory bookmarkStory;

        public BookmarkTestData()
        {
            this.UserId = "test@test.com";
            this.GeoMapId = CreateUUID();
            this.GraphId = CreateUUID();
            this.StoryId = CreateUUID();
            SetBookmarkGeoMap();
            SetBookmarkGraph();
            SetBookmarkStory();

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
            CreateQueryableMockSetWithItem(MockBookmarkGeoMapDbSet, bookmarkGeoMap);
            CreateQueryableMockSetWithItem(MockBookmarkGraphDbSet, bookmarkGraph);
            CreateQueryableMockSetWithItem(MockBookmarkStoryDbDSet, bookmarkStory);
            AddDbSetsToMockContext();
        }

        private void CreateEmptyMockDbSets()
        {
            MockBookmarkGeoMapDbSet = new Mock<DbSet<BookmarkGeoMap>>();
            MockBookmarkGraphDbSet = new Mock<DbSet<BookmarkGraph>>();
            MockBookmarkStoryDbDSet = new Mock<DbSet<BookmarkStory>>();

            CreateQueryableMockDbSet(MockBookmarkGeoMapDbSet, new List<BookmarkGeoMap>());
            CreateQueryableMockDbSet(MockBookmarkGraphDbSet, new List<BookmarkGraph>());
            CreateQueryableMockDbSet(MockBookmarkStoryDbDSet, new List<BookmarkStory>());
        }

        private void AddDbSetsToMockContext()
        {
            MockContext.Setup(m => m.BookmarkGeoMap).Returns(MockBookmarkGeoMapDbSet.Object);
            MockContext.Setup(m => m.BookmarkGraph).Returns(MockBookmarkGraphDbSet.Object);
            MockContext.Setup(m => m.BookmarkStory).Returns(MockBookmarkStoryDbDSet.Object);
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

        public void SetBookmarkGeoMap()
        {
            this.bookmarkGeoMap = new BookmarkGeoMap
            {
                UserId = UserId,
                GeoMapId = GeoMapId
            };
        }

        public void SetBookmarkGraph()
        {
            this.bookmarkGraph = new BookmarkGraph
            {
                UserId = UserId,
                GraphId = GraphId
            };
        }

        public void SetBookmarkStory()
        {
            this.bookmarkStory = new BookmarkStory
            {
                UserId = UserId,
                StoryId = StoryId
            };
        }

        private static string CreateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
