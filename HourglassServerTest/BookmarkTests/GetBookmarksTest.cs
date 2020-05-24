using System;
using System.Collections.Generic;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.BookmarkOperations;
using HourglassServerTest.BookmarkTests;
using HourglassServerTest.StoryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class GetBookmarksTest
    {
        [TestMethod]
        public void TestGetStoryBookmarks()
        {
            BookmarkTestData testData = new BookmarkTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //GeoMap
            List<string> geoMapIds = BookmarkRetriever.GetBookmarkGeoMapByUserId(mockContext, testData.UserId);
            Assert.AreEqual(1, geoMapIds.Count);
            Assert.AreEqual(testData.GeoMapId, geoMapIds[0]);

            //Graph
            List<string> graphIds = BookmarkRetriever.GetBookmarkGraphByUserId(mockContext, testData.UserId);
            Assert.AreEqual(1, graphIds.Count);
            Assert.AreEqual(testData.GraphId, graphIds[0]);

            //Story
            List<string> storyIds = BookmarkRetriever.GetBookmarkStoryByUserId(mockContext, testData.UserId);
            Assert.AreEqual(1, storyIds.Count);
            Assert.AreEqual(testData.StoryId, storyIds[0]);
        }
    }
}
