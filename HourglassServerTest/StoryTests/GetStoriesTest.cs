using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;
using HourglassServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class GetStoriesTest
    {
        [TestMethod]
        public void TestGetStoriesFromStoryRetrieverContainingSampleData()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContent = sampleData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(mockContent, PublicationStatus.PUBLISHED);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            StoryApplicationModel story = stories[0];
            int expectedStoryBlockCount = 3;
            GeneralAssertions.AssertListHasCount(story.StoryBlocks, expectedStoryBlockCount);
            for (int i=0;i<expectedStoryBlockCount; i++) //Checks that blocks are sorted.
                Assert.AreEqual(i, story.StoryBlocks[i].BlockPosition);
        }

        [TestMethod]
        public void TestGetStoriesFromControllerContainingSampleData()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContext = sampleData.GetMockContext();
            StoryController storyController = new StoryController(mockContext);
            var okResult = storyController.GetStoriesInPublished() as OkObjectResult;
            IList<StoryApplicationModel> stories = okResult.Value as List<StoryApplicationModel>;

            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
            GeneralAssertions.AssertListHasMinimumCount(stories[0].StoryBlocks, 1);
        }

        [TestMethod]
        public async Task TestGetStoryByIdFromStoryControllerContainingStory()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContext = sampleData.GetMockContext();
            StoryApplicationModel story = StoryModelRetriever.GetStoryApplicationModelById(mockContext, sampleData.StoryId);
            GeneralAssertions.AssertListHasMinimumCount(story.StoryBlocks, 1);
            Assert.AreEqual(sampleData.StoryId, story.Id);
        }

        [TestMethod]
        public void TestStoryInStatusPublished()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContent = sampleData.GetMockContext();
            Story story = mockContent.Story.ToList()[0];
            bool result = StoryFactory.StoryIsInStatus(story, PublicationStatus.PUBLISHED);
            Assert.IsTrue(result);

            story.PublicationStatus = PublicationStatus.DRAFT.ToString();
            result = StoryFactory.StoryIsInStatus(story, PublicationStatus.PUBLISHED);
            Assert.IsFalse(result);

            story.PublicationStatus = PublicationStatus.REVIEW.ToString();
            result = StoryFactory.StoryIsInStatus(story, PublicationStatus.PUBLISHED);
            Assert.IsFalse(result);
        }
    }
}
