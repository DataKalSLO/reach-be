using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;
using HourglassServer.Controllers;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class GetStoriesTest
    {

        [TestMethod]
        public void GetAllStoriesFromRetriever()
        {
            StoryTestData SampleData = new StoryTestData();
            HourglassContext context = SampleData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetAllStoryApplicationModels(context);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);

        }

        [TestMethod]
        public void GetAllStoriesFromController()
        {
            StoryTestData SampleData = new StoryTestData();
            HourglassContext context = SampleData.GetMockContext();
            StoryController storyController = new StoryController(context);
            IList<StoryApplicationModel> stories = storyController.GetAllStories();

            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);
        }

        [TestMethod]
        public void GetStoryWithIdFromController()
        {
            StoryTestData SampleData = new StoryTestData();
            HourglassContext context = SampleData.GetMockContext();
            StoryController storyController = new StoryController(context);
            StoryApplicationModel story = storyController.GetStoryWithId(SampleData.StoryId);
            Assert.IsNotNull(story);
            GeneralAssertions.AssertListHasMinimumCount(story.StoryBlocks, 1);
            Assert.AreEqual(SampleData.StoryId, story.Id);
        }
    }
}
