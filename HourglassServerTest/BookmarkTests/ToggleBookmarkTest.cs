using System;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServerTest.StoryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class ToggleBookmarkTest
    {
        [TestMethod]
        public void TestToggleBookmark()
        {
            BookmarkTestData testData = new BookmarkTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //GeoMap - Turn off and on
            ToggleState geoMapBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsFalse(geoMapBookmarkState.Enabled);
            geoMapBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsTrue(geoMapBookmarkState.Enabled);

            //Graph - Turn off and on
            ToggleState graphBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsFalse(graphBookmarkState.Enabled);
            graphBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsTrue(graphBookmarkState.Enabled);

            //Story - Turn off and on
            ToggleState storyBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsFalse(storyBookmarkState.Enabled);
            storyBookmarkState = Toggler.ToggleEntity(mockContext.BookmarkGeoMap, testData.bookmarkGeoMap);
            Assert.IsTrue(storyBookmarkState.Enabled);
        }
    }
}
