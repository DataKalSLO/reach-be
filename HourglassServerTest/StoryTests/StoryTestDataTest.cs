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
        private StoryTestData sampleData;
        private HourglassContext mockContext;
        private Story exampleStory;
        private TextBlock exampleTextBlock;
        private GraphBlock exampleGraphBlock;
        private GeoMapBlock exampleGeoMapBlock;
        private ImageBlock exampleImageBlock;

        [TestInitialize]
        public void TestInit()
        {
            sampleData = new StoryTestData();
            mockContext = sampleData.GetMockContext();
            exampleStory = new Story() { StoryId = System.Guid.NewGuid().ToString() };
            exampleTextBlock = new TextBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleGraphBlock = new GraphBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleGeoMapBlock = new GeoMapBlock() { BlockId = System.Guid.NewGuid().ToString() };
            exampleImageBlock = new ImageBlock() { BlockId = System.Guid.NewGuid().ToString() };
        }

        [TestMethod]
        public void DbSetsCanGetData()
        {
            //Story
            List<Story> Stories = mockContext.Story.ToList();
        
            GeneralAssertions.AssertListHasMinimumCount(Stories, 1);
            Assert.AreEqual(sampleData.StoryId, Stories[0].StoryId, "Story");

            //Story Blocks
            List<GeoMapBlock> GeoMapBlocks = mockContext.GeoMapBlock.ToList();
            List<GraphBlock> GraphBlocks = mockContext.GraphBlock.ToList();
            List<ImageBlock> ImageBlocks = mockContext.ImageBlock.ToList();
            List<TextBlock> TextBlocks = mockContext.TextBlock.ToList();

            GeneralAssertions.AssertListHasMinimumCount(GeoMapBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(GraphBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(ImageBlocks, 1);
            GeneralAssertions.AssertListHasMinimumCount(TextBlocks, 1);
            
            Assert.AreEqual(sampleData.GeoMapBlockId, GeoMapBlocks[0].BlockId, "GeoMapBlock");
            Assert.AreEqual(sampleData.GraphBlockId, GraphBlocks[0].BlockId, "GraphBlock");
            Assert.AreEqual(sampleData.ImageUrl, ImageBlocks[0].ImageUrl, "ImageBlock");
            Assert.AreEqual(sampleData.TextBlockId, TextBlocks[0].BlockId, "TextBlock");
        }

        [TestMethod]
        public void DbSetsCanAddData()
        {
            sampleData.ClearDataInContext();

            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.ImageBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 0);

            mockContext.Story.Add(exampleStory);
            mockContext.GeoMapBlock.Add(exampleGeoMapBlock);
            mockContext.GraphBlock.Add(exampleGraphBlock);
            mockContext.ImageBlock.Add(exampleImageBlock);
            mockContext.TextBlock.Add(exampleTextBlock);

            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.ImageBlock, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 1);
        }

        [TestMethod]
        public void DbSetsCanDeleteData()
        {
            DbSetsCanAddData();

            mockContext.Story.Remove(exampleStory);
            mockContext.GeoMapBlock.Remove(exampleGeoMapBlock);
            mockContext.GraphBlock.Remove(exampleGraphBlock);
            mockContext.ImageBlock.Remove(exampleImageBlock);
            mockContext.TextBlock.Remove(exampleTextBlock);

            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.ImageBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 0);
        }
    }
}
