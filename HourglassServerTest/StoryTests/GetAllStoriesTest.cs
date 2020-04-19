using System;
using System.Linq;
using System.Diagnostics;
﻿using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;
using HourglassServer.Controllers;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class GetAllStoriesTest
    {
        StoryTestData SampleData;
        HourglassContext MockContext;

        [TestInitialize]
        public void TestInit()
        {
            SampleData = new StoryTestData();
            MockContext = SampleData.GetMockContext();
        }

        [TestMethod]
        public void GetAllStories()
        {
            HourglassContext context = SampleData.GetMockContext();

            IList<StoryApplicationModel> stories = StoryModelRetriever.GetAllStoryApplicationModels(context);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            StoryApplicationModel story = stories[0];

            Assert.AreEqual(3, story.StoryBlocks.Count);
            Assert.AreEqual(SampleData.StoryId, story.Id);
        }
    }
}
