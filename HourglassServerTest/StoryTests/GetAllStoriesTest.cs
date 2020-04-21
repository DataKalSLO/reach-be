using HourglassServer.Data;
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
        StoryTestData testData;
        HourglassContext MockContext;

        [TestInitialize]
        public void TestInit()
        {
            testData = new StoryTestData();
            testData.AddItemToMockContext();
            MockContext = testData.GetMockContext();
        }

        [TestMethod]
        public void GetAllStoriesFromRetriever()
        {
            HourglassContext context = testData.GetMockContext();

            IList<StoryApplicationModel> stories = StoryModelRetriever.GetAllStoryApplicationModels(context);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);

        }

        [TestMethod]
        public void GetAllStoriesFromController()
        {
            HourglassContext context = testData.GetMockContext();
            StoryController storyController = new StoryController(context);
            IList<StoryApplicationModel> stories = storyController.GetAllStories();

            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);
        }
    }
}
