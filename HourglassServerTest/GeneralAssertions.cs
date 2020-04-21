using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Used to give tests more readable code.
namespace HourglassServerTest
{
    public class GeneralAssertions
    {
        public static void AssertDbSetHasCountMinimum<T>(DbSet<T> dbSet, int minimumCount) where T : class
        {
            Assert.IsNotNull(dbSet);
            Assert.IsTrue(dbSet.Count() >= minimumCount);
        }

        public static void AssertDbSetHasCount<T>(DbSet<T> dbSet, int expectedCount) where T : class
        {
            Assert.IsNotNull(dbSet);
            Assert.AreEqual(expectedCount, dbSet.Count());
        }

        public static void AssertListHasMinimumCount<T>(IList<T> list, int expectedMinimum) where T : class
        {
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= expectedMinimum, typeof(T).FullName + " does not contain an item.");
        }

        public static void AssertListHasCount<T>(IList<T> list, int expectedCount) where T : class
        {
            Assert.IsNotNull(list);
            Assert.AreEqual(expectedCount, list.Count, typeof(T).FullName + " does not contain an item.");
        }
    }
}
