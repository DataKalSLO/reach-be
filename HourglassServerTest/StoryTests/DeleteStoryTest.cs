using System;
using HourglassServer.Data;
using HourglassServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using HourglassServer.Models.Persistent;

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
            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 1);

            StoryController storyController = new StoryController(mockContext);
            storyController.DeleteStoryById(testData.StoryId);
            testData.StoryDbSet.Verify(mock => mock.Remove(It.IsAny<Story>()), Times.Once());
            testData.TextBlockDbSet.Verify(mock => mock.Remove(It.IsAny<TextBlock>()), Times.Once());
            testData.GraphBlockDbSet.Verify(mock => mock.Remove(It.IsAny<GraphBlock>()), Times.Once());
            testData.GeoMapBlockDbDSet.Verify(mock => mock.Remove(It.IsAny<GeoMapBlock>()), Times.Once());
        }
    }
}
