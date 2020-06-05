using Moq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryOperations;
using HourglassServer.Data.Application.StoryModel;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class UpdateStoryTest
    {
        [TestMethod]
        public void UpdateStoryTextBlock()
        {
            StoryTestData testData = new StoryTestData();
            string newTitle = "This is a new title";
            string newEditorState = "HelloThisIsAnEditorState";
            StoryBlockModel updatedTextBlock = new StoryBlockModel()
            {
                Id = testData.TextBlockId,
                Type = StoryBlockType.TEXTDB,
                EditorState = newEditorState
            };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel> { updatedTextBlock };
            StoryApplicationModel story = new StoryApplicationModel()
            {
                Id = testData.StoryId,
                Title = newTitle,
                StoryBlocks = storyBlocks
            };
            HourglassContext mockContext = testData.GetMockContext();
            
            StoryModelUpdater.UpdateStoryApplicationModel(mockContext, story);

            List<Story> stories = mockContext.Story.ToList();
            GeneralAssertions.AssertListHasCount(stories, 1);
            Story testStory = stories[0];
            Assert.AreEqual(testData.StoryId, testStory.StoryId);

            testData.MockStoryDbSet.Verify(mock => mock.Update(It.IsAny<Story>()), Times.AtLeastOnce());
            testData.MockTextBlockDbSet.Verify(mock => mock.Update(It.IsAny<TextBlock>()), Times.AtLeastOnce());
            testData.MockTextBlockDbSet.Verify(mock => mock.Add(It.IsAny<TextBlock>()), Times.Never()); // Nothing new being added
        }

        [TestMethod]
        public void UpdateStoryImageBlock()
        {
            StoryTestData testData = new StoryTestData();
            ImageBlock newImageBlock = testData.CreateImageBlock();
            string newUrl = "Https://localhost:5000/newURL";
            newImageBlock.ImageUrl = newUrl;

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel> { new StoryBlockModel(newImageBlock) };
            StoryApplicationModel story = new StoryApplicationModel()
            {
                Id = testData.StoryId,
                StoryBlocks = storyBlocks
            };
            HourglassContext mockContext = testData.GetMockContext();

            StoryModelUpdater.UpdateStoryApplicationModel(mockContext, story);

            List<Story> stories = mockContext.Story.ToList();
            GeneralAssertions.AssertListHasCount(stories, 1);
            Story testStory = stories[0];
            Assert.AreEqual(testData.StoryId, testStory.StoryId);

            testData.MockStoryDbSet.Verify(mock => mock.Update(It.IsAny<Story>()), Times.AtLeastOnce());
            testData.MockImageBlockDbSet.Verify(mock => mock.Update(It.IsAny<ImageBlock>()), Times.AtLeastOnce());
            testData.MockImageBlockDbSet.Verify(mock => mock.Add(It.IsAny<ImageBlock>()), Times.Never()); // Nothing new being added
        }

        [TestMethod]
        public void UpdateStoryFromControllerContainingNewTextBlock()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();

            string newTitle = "This is a new title";
            string newEditorState = "HelloThisIsAnEditorState";

            StoryBlockModel newTextBlock = new StoryBlockModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Type = StoryBlockType.TEXTDB,
                EditorState = newEditorState
            };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel> { newTextBlock };
            StoryApplicationModel story = new StoryApplicationModel()
            {
                Id = testData.StoryId,
                Title = newTitle,
                StoryBlocks = storyBlocks
            };

            StoryModelUpdater.UpdateStoryApplicationModel(mockContext, story);

            testData.MockStoryDbSet.Verify(mock => mock.Update(It.IsAny<Story>()), Times.AtLeastOnce());
            testData.MockTextBlockDbSet.Verify(mock => mock.Update(It.IsAny<TextBlock>()), Times.Never());
            testData.MockTextBlockDbSet.Verify(mock => mock.Add(It.IsAny<TextBlock>()), Times.AtLeastOnce()); // For new TextBlock
        }

        //TODO: When `Find` and `Any` are mocked test that story blocks get deleted. Testing in postman for time being.
    }
}
