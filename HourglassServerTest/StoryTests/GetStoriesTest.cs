using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.DataManipulation.StoryOperations;
using System.Collections.Generic;

/* Note, the reason some of these tests are so similar and not 
 * 
 */
namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class GetStoriesTest
    {
        [TestMethod]
        public void TestGetStoryByIdFromStoryControllerContainingStory()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContext = sampleData.GetMockContext();
            StoryApplicationModel story = StoryModelRetriever.GetStoryApplicationModelById(mockContext, sampleData.StoryId);

            //VP 1 - Correct story returned
            Assert.AreEqual(sampleData.StoryId, story.Id);

            //VP 2 - Story blocks returned with story
            int expectedStoryBlockCount = 4;
            GeneralAssertions.AssertListHasCount(story.StoryBlocks, expectedStoryBlockCount);
            for (int i = 0; i < expectedStoryBlockCount; i++) //Checks that blocks are sorted.
                Assert.AreEqual(i, story.StoryBlocks[i].BlockPosition);
        }

        [TestMethod]
        public void TestGetStoriesInDraftStatus()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(mockContext, PublicationStatus.DRAFT);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
        }

        [TestMethod]
        public void TestGetStoriesInDraftStatusByUserId()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatusByUserId(mockContext, PublicationStatus.DRAFT, testData.UserId);
            GeneralAssertions.AssertListHasMinimumCount(stories, 1);
        }

        [TestMethod]
        public void TestGetStoriesInReviewStatus()
        {
            // TODO: Refactor test data to include at least one story in review
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(mockContext, PublicationStatus.REVIEW);
            GeneralAssertions.AssertListHasCount(stories, 0);
        }

        [TestMethod]
        public void TestGetStoriesInReviewStatusByUserId()
        {
            // TODO: Refactor test data to include at least one story in review
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatusByUserId(mockContext, PublicationStatus.REVIEW, testData.UserId);
            GeneralAssertions.AssertListHasMinimumCount(stories, 0);
        }

        [TestMethod]
        public void TestGetStoriesInPublishedStatus()
        {
            // TODO: Refactor test data to include at least one published story
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatus(mockContext, PublicationStatus.PUBLISHED);
            GeneralAssertions.AssertListHasCount(stories, 0);
        }

        [TestMethod]
        public void TestGetStoriesInPublishedStatusByUserId()
        {
            // TODO: Refactor test data to include at least one published story
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();
            IList<StoryApplicationModel> stories = StoryModelRetriever.GetStoryApplicationModelsInPublicationStatusByUserId(mockContext, PublicationStatus.PUBLISHED, testData.UserId);
            GeneralAssertions.AssertListHasMinimumCount(stories, 0);
        }
    }
}
