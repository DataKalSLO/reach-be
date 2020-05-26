using Moq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using System.Collections.Generic;
using HourglassServer.Data.DataManipulation.GraphOperations;

namespace HourglassServerTest.GraphTests
{
    [TestClass]
    public class GraphModelRetrieverTest
    {
        [TestMethod]
        public void getListofApplicationsEmptyList()
        {
            List<Graph> testData = new List<Graph>();
            var result =  GraphModelRetriever.getListOfApplicationModel(testData);
            Assert.AreEqual(result.Count, 0);
        }
    }
}