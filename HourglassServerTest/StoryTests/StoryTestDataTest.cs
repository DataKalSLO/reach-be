using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class StoryTestDataTest
    {
        StoryTestData sampleData;
        HourglassContext mockContext;

        [TestInitialize]
        public void TestInit()
        {
            sampleData = new StoryTestData();
            mockContext = sampleData.GetMockContext();
        }

        [TestMethod]
        public void DbSetsHaveDataLoaded()
        {
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

            Assert.AreEqual(sampleData.StoryId, Stories[0].StoryId, "Story");
            Assert.AreEqual(sampleData.TextBlockId, TextBlocks[0].BlockId, "TextBlock");
            Assert.AreEqual(sampleData.GeoMapBlockId, GeoMapBlocks[0].BlockId, "GeoMapBlock");
            Assert.AreEqual(sampleData.GraphBlockId, GraphBlocks[0].BlockId, "GraphBlock");
            Assert.AreEqual(sampleData.StoryId, StoryBlocks[0].StoryId, "StoryBlock");
        }
    }
}
