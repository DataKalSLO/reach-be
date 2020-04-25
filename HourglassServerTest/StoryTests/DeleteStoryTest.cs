using System;
using HourglassServer.Data;
using HourglassServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class DeleteStoryTest
    {
        [TestMethod]
        public void TestDeleteStoryByIdFromController()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            StoryController storyController = new StoryController(mockContext);
            storyController.DeleteStoryById(testData.StoryId);
            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
        }
    }
}
