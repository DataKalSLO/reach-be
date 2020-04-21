using Moq;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

/* This files contains a context with the following items
 * 1 Story bookmark, 1 graph bookmark, and 1 map bookmark
 */
namespace HourglassServerTest
{
    public abstract class TestData
    {

        protected Mock<HourglassContext> MockContext;

        public TestData()
        {
            MockContext = new Mock<HourglassContext>();
        }

        public HourglassContext GetMockContext()
        {
            return MockContext.Object;
        }

        public void ClearDataInContext()
        {
            InitializeMockSets();
            AddDbSetsToMockContext();
        }


        public static string CreateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public abstract void AddItemToMockContext();
        public abstract void InitializeMockSets();
        protected abstract void AddDbSetsToMockContext();

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
            //TODO: Find a way to moq updating items in list.
            return mockSet;
        }
    }
}
