using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryOperations;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using System.Linq;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class CreateStoryTest
    {
        const string EditorState = "{\"TheMeaningOfLife\": 42}";

        StoryApplicationModel exampleStory;
        const string StoryId = "fa370adf-ffcc-4b2d-9d78-f89b588e9714";
        const string BlockId = "18f84b62-c1fc-45f6-a251-3a938666930d";
        const string exampleTitle = "This is an example title";
        const string exampleDescription = "This is an example description";
        [TestInitialize]
        public void InitTest()
        {
            StoryBlockModel newTextBlock = new StoryBlockModel()
            {
                Id = BlockId,
                EditorState = EditorState,
                Type = StoryBlockType.TEXTDB
            };
            exampleStory = new StoryApplicationModel()
            {
                Id = StoryId,
                Title = exampleTitle,
                Description = exampleDescription,
                StoryBlocks = new List<StoryBlockModel>() { newTextBlock }
            };
        }

        [TestMethod]
        public void CreateTestWithCreator()
        {
            StoryTestData sampleData = new StoryTestData();
            HourglassContext mockContext = sampleData.GetMockContext();
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(mockContext, exampleStory);

            Story story = mockContext.Story
                .Where(story => story.StoryId == StoryId)
                .Single();

            //Story meta information
            Assert.AreEqual(exampleTitle, story.Title);
            Assert.AreEqual(exampleDescription, story.Description);

            string InitialDate = "0001-01-01T00:00:00";
            Assert.AreNotEqual(InitialDate, story.DateCreated);
            Assert.AreNotEqual(InitialDate, story.DateLastEdited);

            //Story Blocks
            TextBlock textBlock = mockContext.TextBlock
               .Where(textBlock => textBlock.BlockId == BlockId).Single();

            Assert.AreEqual(EditorState, textBlock.EditorState);
        }
    }
}
