using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using System.Collections.Generic;

/* Tests the Test Data. In order reliably test my https routes I assume that
 * the sample data can get, add, and remove data without fail. This ensures that.
 */
namespace HourglassServerTest.BookmarkTests
{
    [TestClass]
    public class BookmarkTestDataTest
    {
        private BookmarkTestData testData;

        [TestInitialize]
        public void TestInit()
        {
            testData = new BookmarkTestData();
        }

        [TestMethod]
        public void DbSetsCanGetData()
        {
            testData.AddItemToMockContext();
            HourglassContext mockContext = testData.GetMockContext();

            List<StoryBookmark> StoryBookmarks = mockContext.StoryBookmark.ToList();
            List<GraphBookmark> GraphBookmarks = mockContext.GraphBookmark.ToList();
            List<GeoMapBookmark> GeoMapBookmarks = mockContext.GeoMapBookmark.ToList();
            List<Graph> Graphs = mockContext.Graph.ToList(); //TODO: Remove when GraphTestData exists
            List<GeoMap> GeoMaps = mockContext.GeoMap.ToList(); //TODO: Remove when GeoMapTestData exists

            GeneralAssertions.AssertListHasMinimumCount(StoryBookmarks, 1);
            GeneralAssertions.AssertListHasMinimumCount(GraphBookmarks, 1);
            GeneralAssertions.AssertListHasMinimumCount(GeoMapBookmarks, 1);
            GeneralAssertions.AssertListHasMinimumCount(Graphs, 1);
            GeneralAssertions.AssertListHasMinimumCount(GeoMaps, 1);
        }

        [TestMethod]
        public void DbSetsCanAddData()
        {
            testData.ClearDataInContext();
            HourglassContext mockContext = testData.GetMockContext();

            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBookmark, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBookmark, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBookmark, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.Graph, 0); //TODO: Remove when GraphTestData exists
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMap, 0); //TODO: Remove when GeoMapTestData exists

            mockContext.StoryBookmark.Add(testData.GetStoryBookmark());
            mockContext.GraphBookmark.Add(testData.GetGraphBookmark());
            mockContext.GeoMapBookmark.Add(testData.GetGeoMapBookmark());
            mockContext.Graph.Add(testData.GetGraph()); //TODO: remove when GraphTestData exists
            mockContext.GeoMap.Add(testData.GetGeoMap()); //TODO: remove when GeoMapTestData exists

            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBookmark, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBookmark, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBookmark, 1);
            GeneralAssertions.AssertDbSetHasCount(mockContext.Graph, 1); //remove when GraphTestData exists
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMap, 1); //remove when GeoMapTestData exists
        }

        [TestMethod]
        public void DbSetsCanDeleteData()
        {
            testData.AddItemToMockContext();
            HourglassContext mockContext = testData.GetMockContext();
            mockContext.StoryBookmark.Remove(testData.GetStoryBookmark());
            mockContext.GraphBookmark.Remove(testData.GetGraphBookmark());
            mockContext.GeoMapBookmark.Remove(testData.GetGeoMapBookmark());
            mockContext.Graph.Remove(testData.GetGraph()); //TODO: remove when GraphTestData exists
            mockContext.GeoMap.Remove(testData.GetGeoMap()); //TODO: Remove when GeoMapTestData exists
            GeneralAssertions.AssertDbSetHasCount(mockContext.Story, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.TextBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GraphBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.GeoMapBlock, 0);
            GeneralAssertions.AssertDbSetHasCount(mockContext.StoryBlock, 2);
        }
    }
}
