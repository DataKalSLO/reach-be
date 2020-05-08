using HourglassServer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HourglassServer.Models.Persistent;

namespace HourglassServerTest
{
    [TestClass]
    public class QueryFormatUtilTest
    {
        private List<DatasetMetaData> _metadata = new List<DatasetMetaData>()
        {
            new DatasetMetaData() {
                TableName = "table_1",
                ColumnNames = new String[] {"col_1"},
                DataTypes = new String[] {"type_1"}
            },
            new DatasetMetaData() {
                TableName = "table_2",
                ColumnNames = new String[] {"col_a, col_b"},
                DataTypes = new String[] {"type_2", "type_2"}
            }
        };

        [TestMethod]
        public void TestFormatSelectFullDatasetQuery()
        {
            var util = new QueryFormatUtil();

            var tableName = "table_1";
            var expectedResult = true;
            var expectedQuery = "SELECT * FROM datasets.table_1";

            var result = util.formatSelectFullDatasetQuery(tableName, _metadata);

            Assert.AreEqual(result, expectedResult);
            Assert.AreEqual(util.getQuery(), expectedQuery);
        }

        [TestMethod]
        public void TestFormatSelectFullDatasetQuery_TableDoesntExist()
        {
            var util = new QueryFormatUtil();

            var tableName = "non_existant";
            var expectedResult = false;

            var result = util.formatSelectFullDatasetQuery(tableName, _metadata);

            Assert.AreEqual(result, expectedResult);
            Assert.ThrowsException<InvalidOperationException>(util.getQuery);
            Assert.IsNotNull(util.Error);
        }

        [TestMethod]
        public void TestFormatSelectFullDatasetQuery_NoMetadata()
        {
            var util = new QueryFormatUtil();      
            List<DatasetMetaData> emptyMetadata = new List<DatasetMetaData>();

            var expectedResult = false;
            var result = util.formatSelectFullDatasetQuery("some_table", emptyMetadata);

            Assert.AreEqual(result, expectedResult);
            Assert.IsNotNull(util.Error);
        }
    }
}