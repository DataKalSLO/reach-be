using Moq;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HourglassServerTest
{
    public abstract class TestData
    {
        public readonly Mock<HourglassContext> MockContext;

        public TestData()
        {
            MockContext = new Mock<HourglassContext>();
            CreateEmptyMockDbSets();
        }

        public HourglassContext GetMockContext()
        {
            return MockContext.Object;
        }

        public void ClearDataInContext()
        {
            CreateEmptyMockDbSets();
            AddDbSetsToMockContext();
        }

        protected Mock<DbSet<T>> CreateQueryableMockSetWithItem<T>(Mock<DbSet<T>> mockSet, T item) where T : class
        {
            List<T> items = new List<T>() { item };
            return CreateQueryableMockDbSet(mockSet, items);
        }

        protected Mock<DbSet<T>> CreateQueryableMockDbSet<T>(Mock<DbSet<T>> mockSet, List<T> sourceList) where T : class
        {
            IQueryable<T> queryableList = sourceList.AsQueryable();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            //TODO: Find a way to mock both `Find` AND `Any` DbSet methods for testing updating stories.
            //TODO: Find a way to moq updating items in list.
            return mockSet;
        }

        public static string CreateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        protected abstract void CreateEmptyMockDbSets();
        protected abstract void AddDbSetsToMockContext();
    }
}
