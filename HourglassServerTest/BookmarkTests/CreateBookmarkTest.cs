using System;
using HourglassServer.Data.BookmarkModel;
using HourglassServer.Controllers;
using HourglassServer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class CreateBookmarkTest
    {
        [TestMethod]
        public void CreateBookFromController()
        {
            BookmarkTestData testData = new BookmarkTestData();
            testData.ClearDataInContext();
            HourglassContext context = testData.GetMockContext();
            GeneralAssertions.AssertDbSetHasCount(context.StoryBookmark, 0);

            BookmarkController bookmarkController = new BookmarkController(context);
            Bookmark bookmark = new Bookmark()
            {
                UserId = testData.UserId,
                ItemId = testData.StoryId,
                Type = ContentType.STORY
            }; 
            bookmarkController.CreateBookmark(bookmark);
            GeneralAssertions.AssertDbSetHasCount(context.StoryBookmark, 1);
        }
    }
}
