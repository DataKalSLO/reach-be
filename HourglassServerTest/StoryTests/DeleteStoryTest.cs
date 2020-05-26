using Moq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Controllers;
using HourglassServer.Data.Application.StoryModel;
using System.Linq;

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

            StoryModelDeleter.DeleteStoryById(mockContext, testData.StoryId);
            testData.StoryDbSet.Verify(mock => mock.Remove(It.IsAny<Story>()), Times.Once());
            testData.TextBlockDbSet.Verify(mock => mock.RemoveRange(It.IsAny<IQueryable<TextBlock>>()), Times.Once());
            testData.GraphBlockDbSet.Verify(mock => mock.RemoveRange(It.IsAny<IQueryable<GraphBlock>>()), Times.Once());
            testData.GeoMapBlockDbDSet.Verify(mock => mock.RemoveRange(It.IsAny<IQueryable<GeoMapBlock>>()), Times.Once());
        }
    }
}
