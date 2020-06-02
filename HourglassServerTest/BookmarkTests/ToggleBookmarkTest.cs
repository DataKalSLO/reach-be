using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServerTest.StoryTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class ToggleBookmarkTest
    {
        [TestMethod]
        public void TestTurnBookmarksOffAndOn()
        {
            BookmarkTestData testData = new BookmarkTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //GeoMaps, Graphs, Story
            TestToggle(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap, true);
            TestToggle(mockContext.BookmarkGraph, testData.bookmarkGraph, true);
            TestToggle(mockContext.BookmarkStory, testData.bookmarkStory, true);
        }

        private void TestToggle<T>(DbSet<T> setToToggle, T entity, bool entityExists) where T : class
        {
            ToggleState geoMapBookmarkState = Toggler.ToggleEntity<T>(setToToggle, entity);
            Assert.AreEqual(!entityExists, geoMapBookmarkState.Enabled);
            geoMapBookmarkState = Toggler.ToggleEntity<T>(setToToggle, entity);
            Assert.AreEqual(entityExists, geoMapBookmarkState.Enabled);
        }
    }
}
