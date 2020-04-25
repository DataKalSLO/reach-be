using System;
using Moq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using HourglassServer.Controllers;
using HourglassServer.Data.Application.StoryModel;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class UpdateStoryTest
    {
        [TestMethod]
        public void UpdateStoryFromControllerContainingStory()
        {
            StoryTestData testData = new StoryTestData();
            string newTitle = "This is a new title";
            string newEditorState = "HelloThisIsAnEditorState";
            StoryBlockModel newTextBlock = new StoryBlockModel()
            {
                Id = System.Guid.NewGuid().ToString(), //TODO: Replace with call to test data after refactor PR
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
            HourglassContext mockContext = testData.GetMockContext();
            
            StoryController storyController = new StoryController(mockContext);
            storyController.UpdateStory(story);

            List<Story> stories = mockContext.Story.ToList();
            GeneralAssertions.AssertListHasCount(stories, 1);
            Story testStory = stories[0];
            Assert.AreEqual(testData.StoryId, testStory.StoryId);

            testData.StoryDbSet.Verify(mock => mock.Update(It.IsAny<Story>()), Times.Once());
            testData.TextBlockDbSet.Verify(mock => mock.Update(It.IsAny<TextBlock>()), Times.Once());
            testData.StoryBlockDbSet.Verify(mock => mock.Update(It.IsAny<StoryBlock>()), Times.Once());
        }
    }
}
