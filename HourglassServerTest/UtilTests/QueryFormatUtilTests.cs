using HourglassServer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HourglassServer.Data.Application.MetadataModel;

namespace HourglassServerTest
{
    [TestClass]
    public class QueryFormatUtilTest
    {
        private List<MetadataApplicationModel> _metadata = new List<MetadataApplicationModel>()
        {
            new MetadataApplicationModel() {
                TableName = "table_1",
                ColumnNames = new String[] {"col_1"},
                DataTypes = new String[] {"type_1"}
            },
            new MetadataApplicationModel() {
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
            var columns = new[] { "string_1", "string_2" };
            var expectedResult = true;
            var expectedQuery = "SELECT string_1, string_2 FROM datasets.table_1";
            var meta_data = new MetadataApplicationModel
            {
                TableName = tableName,
                ColumnNames = columns,
                DataTypes = new[] { "int", "string" },
                GeoType = "none"
            };

            var result = util.formatTableQuery(tableName, _metadata);
            var result2 = util.createQuery(meta_data, columns);


            Assert.AreEqual(result, expectedResult);
            Assert.AreEqual(util.getQuery(), expectedQuery);
        }

        [TestMethod]
        public void TestFormatSelectFullDatasetQuery_TableDoesntExist()
        {
            var util = new QueryFormatUtil();

            var tableName = "non_existant";
            var expectedResult = false;

            var result = util.formatTableQuery(tableName, _metadata);

            Assert.AreEqual(result, expectedResult);
            Assert.ThrowsException<InvalidOperationException>(util.getQuery);
            Assert.IsNotNull(util.Error);
        }

        [TestMethod]
        public void TestFormatSelectFullDatasetQuery_NoMetadata()
        {
            var util = new QueryFormatUtil();
            List<MetadataApplicationModel> emptyMetadata = new List<MetadataApplicationModel>();

            var expectedResult = false;
            var result = util.formatTableQuery("some_table", emptyMetadata);

            Assert.AreEqual(result, expectedResult);
            Assert.IsNotNull(util.Error);
        }
    }
}