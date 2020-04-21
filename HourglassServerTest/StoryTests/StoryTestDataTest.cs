using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using System.Collections.Generic;

/* Tests the Test Data. In order reliably test my https routes I assume that
 * the sample data can get, add, and remove data without fail. This ensures that.
 */
namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class StoryTestDataTest
    {
        private StoryTestData testData;
        private Story exampleStory;
        private TextBlock exampleTextBlock;
        private GraphBlock exampleGraphBlock;
        private GeoMapBlock exampleGeoMapBlock;
        private StoryBlock exampleStoryBlock;

        [TestInitialize]
        public void TestInit()
        {
            testData = new StoryTestData();
            exampleStory = new Story() { StoryId = System.Guid.NewGuid().ToString() };
            exampleTextBlock = new TextBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleGraphBlock = new GraphBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleGeoMapBlock = new GeoMapBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleStoryBlock = new StoryBlock()
            {
                StoryId = System.Guid.NewGuid().ToString(),
                BlockId = System.Guid.NewGuid().ToString()
            };
        }

        [TestMethod]
        public void DbSetsCanGetData()
        {
            testData.AddItemToMockContext();
            HourglassContext mockContext = testData.GetMockContext();

            List<Story> Stories = mockContext.Story.ToList();
            List<TextBlock> TextBlocks = mockContext.TextBlock.ToList();
            List<GeoMapBlock> GeoMapBlocks = mockContext.GeoMapBlock.ToList();
            List<GraphBlock> GraphBlocks = mockContext.GraphBlock.ToList();
            List<StoryBlock> StoryBlocks = mockContext.StoryBlock.ToList();

            GeneralAssertions.AssertListHasMinimumCount(Stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(TextBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(GeoMapBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(GraphBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(StoryBlocks, 1);

            Assert.AreEqual(testData.StoryId, Stories[0].StoryId, "Story");
            Assert.AreEqual(testData.TextBlockId, TextBlocks[0].BlockId, "TextBlock");
            Assert.AreEqual(testData.GeoMapBlockId, GeoMapBlocks[0].BlockId, "GeoMapBlock");
            Assert.AreEqual(testData.GraphBlockId, GraphBlocks[0].BlockId, "GraphBlock");
            Assert.AreEqual(testData.StoryId, StoryBlocks[0].StoryId, "StoryBlock");
        }

        [TestMethod]
        public void DbSetsCanAddData()
        {
            testData.ClearDataInContext();
            HourglassContext mockContext = testData.GetMockContext();

            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBlock, 0);

            mockContext.Story.Add(exampleStory);
            mockContext.TextBlock.Add(exampleTextBlock);
            mockContext.GraphBlock.Add(exampleGraphBlock);
            mockContext.GeoMapBlock.Add(exampleGeoMapBlock);
            mockContext.StoryBlock.Add(exampleStoryBlock);

            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBlock, 1);
        }

        [TestMethod]
        public void DbSetsCanDeleteData()
        {
            testData.AddItemToMockContext();
            HourglassContext mockContext = testData.GetMockContext();
            mockContext.Story.Remove(testData.Story);
            mockContext.TextBlock.Remove(testData.TextBlock);
            mockContext.GraphBlock.Remove(testData.GraphBlock);
            mockContext.GeoMapBlock.Remove(testData.GeoMapBlock);
            mockContext.StoryBlock.Remove(testData.StoryBlocks[0]);
            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBlock, 2);
        }
    }
}
