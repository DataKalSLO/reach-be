using System;
using HourglassServer.Data.BookmarkModel;
using HourglassServer.Controllers;
using HourglassServer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class GetBookmarkTest
    {
        [TestMethod]
        public void GetUserBookmarksFromController()
        {
            BookmarkTestData testData = new BookmarkTestData();
            testData.AddItemToMockContext();
            HourglassContext context = testData.GetMockContext();
            GeneralAssertions.AssertDbSetHasCount(context.StoryBookmark, 1);
            GeneralAssertions.AssertDbSetHasCount(context.GraphBookmark, 1);
            GeneralAssertions.AssertDbSetHasCount(context.GeoMapBookmark, 1);

            BookmarkController bookmarkController = new BookmarkController(context);
            List<BookmarkContent> bookmarks = bookmarkController.GetBookmarks(testData.UserId);
            GeneralAssertions.AssertListHasCount(bookmarks, 3);
        }
    }
}
