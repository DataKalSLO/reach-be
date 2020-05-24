using System.Linq;
using Moq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;
using HourglassServer.Controllers;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class CreateStoryTest
    {
        StoryTestData sampleData;
        HourglassContext context;
        StoryApplicationModel exampleStory;
        string StoryId = System.Guid.NewGuid().ToString();

        [TestInitialize]
        public void InitTest()
        {
            sampleData = new StoryTestData();
            context = sampleData.GetMockContext();
            StoryBlockModel newTextBlock = new StoryBlockModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                EditorState = "{\"TheMeaningOfLife\": 42}",
                Type = StoryBlockType.TEXTDB
            };
            exampleStory = new StoryApplicationModel()
            {
                Id = StoryId,
                Title = "This is an example title",
                Description = "This is an example description",
                StoryBlocks = new List<StoryBlockModel>() { newTextBlock }
            };
        }

        [TestMethod]
        public void CreateTestWithCreator()
        {
            sampleData.ClearDataInContext();
            StoryModelCreator.AddStoryApplicationModelToDatabaseContext(context, exampleStory);
            AssertItemsCreated();
        }

        public void AssertItemsCreated()
        {
            int count = 1;
            GeneralAssertions.AssertDbSetHasCount(context.TextBlock, count);
            GeneralAssertions.AssertDbSetHasCount(context.Story, count);
        }
    }
}
