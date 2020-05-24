using System;
using HourglassServer.Custom.Constraints;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HourglassServerTest.StoryTests
{
    [TestClass]
    public class StoryContraintCheckerTest
    {
        [TestMethod]
        public void TestStoryExistsWithId()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //Satisfied
            StoryContraintChecker constraintChecker = new StoryContraintChecker(
                new ConstraintEnvironment(null, mockContext),
                new StoryApplicationModel { Id = testData.StoryId });
            Assert.IsTrue(constraintChecker.SatisfiesConstraint(Constraints.STORY_EXISTS_WITH_ID));

            //Violated
            constraintChecker = new StoryContraintChecker(
                new ConstraintEnvironment(null, mockContext),
                new StoryApplicationModel { Id = System.Guid.NewGuid().ToString() });
            Assert.IsFalse(constraintChecker.SatisfiesConstraint(Constraints.STORY_EXISTS_WITH_ID));
        }

        [TestMethod]
        public void TestStoryHasDraftStatus()
        {
            StoryTestData testData = new StoryTestData();
            HourglassContext mockContext = testData.GetMockContext();

            //Note, this test still passes if PublicationStatus is null
            //Converter will choose the first enum when presented with null
            StoryContraintChecker constraintChecker = new StoryContraintChecker(
                new ConstraintEnvironment(null, mockContext),
                new StoryApplicationModel { Id = testData.StoryId, PublicationStatus = PublicationStatus.DRAFT});
            Assert.IsTrue(constraintChecker.SatisfiesConstraint(Constraints.HAS_DRAFT_STATUS));

            constraintChecker = new StoryContraintChecker(
                new ConstraintEnvironment(null, mockContext),
                new StoryApplicationModel { Id = testData.StoryId, PublicationStatus = PublicationStatus.REVIEW });
            Assert.IsFalse(constraintChecker.SatisfiesConstraint(Constraints.HAS_DRAFT_STATUS));
        }
    }
}
