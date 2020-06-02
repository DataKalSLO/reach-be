using System.Collections.Generic;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.BookmarkOperations;
using HourglassServerTest.StoryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class GetBookmarksTest
    {
        [TestMethod]
        public void TestGetBookmarks()
        {
            BookmarkTestData testData = new BookmarkTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //GeoMap
            List<string> geoMapIds = BookmarkRetriever.GetBookmarkGeoMapByUserId(mockContext, testData.UserId);
            GeneralAssertions.AssertListHasCount(geoMapIds, 1);

            //Graph
            List<string> graphIds = BookmarkRetriever.GetBookmarkGraphByUserId(mockContext, testData.UserId);
            GeneralAssertions.AssertListHasCount(graphIds, 1);

            //Story
            List<string> storyIds = BookmarkRetriever.GetBookmarkStoryByUserId(mockContext, testData.UserId);
            GeneralAssertions.AssertListHasCount(storyIds, 1);

        }
    }
}
