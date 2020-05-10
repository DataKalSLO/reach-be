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
        StoryTestData SampleData;
        HourglassContext MockContext;

        [TestInitialize]
        public void TestInit()
        {
            SampleData = new StoryTestData();
            MockContext = SampleData.GetMockContext();
        }

        [TestMethod]
        public void GetAllStoriesFromRetriever()
        {
            HourglassContext context = SampleData.GetMockContext();

            IList<StoryApplicationModel> stories = StoryModelRetriever.GetAllStoryApplicationModels(context);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);

        }

        [TestMethod]
        public void GetAllStoriesFromController()
        {
            HourglassContext context = SampleData.GetMockContext();
            StoryController storyController = new StoryController(context);
            IList<StoryApplicationModel> stories = storyController.GetAllStories();

            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);
        }
    }
}
