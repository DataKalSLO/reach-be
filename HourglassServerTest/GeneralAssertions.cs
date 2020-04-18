using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HourglassServerTest
{
    public class GeneralAssertions
    {
        public static void AssertDbSetCountMinimum<T>(DbSet<T> dbSet, int minimumCount) where T : class
        {
            Assert.IsNotNull(dbSet);
            AssertListHasMinimumCount(dbSet.ToList(), minimumCount);
        }

        public static void AssertListHasMinimumCount<T>(IList<T> list, int expectedMinimum) where T : class
        {
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= expectedMinimum, typeof(T).FullName + " does not contain an item.");
        }
    }
}
