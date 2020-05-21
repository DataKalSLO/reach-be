//using HourglassServer.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System.Linq;
//using System.Collections.Generic;
//using System;
//using HourglassServer.Models.Persistent;
//using HourglassServer.Data.Application.Maps;
//using HourglassServer.Controllers;

//namespace HourglassServerTest.MapTests
//{
//    [TestClass]
//    public class MapControllerTest
//    {
//        private IQueryable<Point> _pdata = new List<Point>()
//        {
//            new Point()
//            {
//                Id = 1,
//                Longitude = (decimal)-120.6912,
//                Latitude = (decimal)35.2659
//            },
//            new Point()
//            {
//                Id = 2,
//                Longitude = (decimal)-120.7401,
//                Latitude = (decimal)35.3292,
//            },
//            new Point ()
//            {
//                Id = 3,
//                Longitude = (decimal)-120.6625,
//                Latitude = (decimal)35.305,
//            }
//        }.AsQueryable();

//        private IQueryable<Location> _data = new List<Location>()
//        {
//        new Location()
//            {
//                Name = "Laguna Lake",
//                TableName = "WaterSources",
//                PointId = 1,
//                TableNameNavigation = new DatasetMetaData()
//                {
//                    TableName = "WaterSources",
//                    ColumnNames = "name, latititude, longitude",
//                    DataTypes = "string, float, float",
//                    //LocationValues = new bool[] { false, true, true }
//                }
//            },
//        new Location()
//            {
//                Name = "Cuesta College",
//                TableName = "Colleges",
//                PointId = 2,
//                TableNameNavigation = new DatasetMetaData()
//                {
//                    TableName = "Colleges",
//                    ColumnNames = "name, latititude, longitude",
//                    DataTypes = "string, float, float",
//                    //LocationValues = new bool[] { false, true, true }
//                }
//            },
//        new Location()
//            {
//                Name = "Cal Poly SLO",
//                TableName = "Colleges",
//                PointId = 3,
//                TableNameNavigation = new DatasetMetaData()
//                {
//                    TableName = "Colleges",
//                    ColumnNames = "name, latititude, longitude",
//                    DataTypes = "string, float, float",
//                    //LocationValues = new bool[] { false, true, true }
//                }
//            },

//        }.AsQueryable();

//        private IQueryable<CensusVariables> _census = new List<CensusVariables>()
//        {
//            new CensusVariables()
//            {
//                Name = "B19019_001E",
//                Description = "MEDIAN HOUSEHOLD INCOME IN THE PAST 12 MONTHS (IN 2018 INFLATIONADJUSTED DOLLARS) BY HOUSEHOLD SIZEEstimate!!Total"
//            }
//        }.AsQueryable();

//        private IQueryable<CensusData> _censusData = new List<CensusData>()
//        {
//            new CensusData()
//            {
//                VariableName = "B19019_001E",
//                Year = 2018,
//                GeoType = "zip",
//                GeoName = "93428",
//                Value = 69766
//            },
//            new CensusData()
//            {
//                VariableName = "B19019_001E",
//                Year = 2018,
//                GeoType = "zip",
//                GeoName = "93402",
//                Value = 75302
//            }
//            //new CensusData()
//            //{
//            //    VariableName = "B19019_001E",
//            //    Year = 2018,
//            //    GeoName = "93455",
//            //    Value = 84404
//            //}
//        }.AsQueryable();

//        private IQueryable<ZipCode> _zipCodeData = new List<ZipCode>()
//        {
//            new ZipCode()
//            {
//                Zip = 93428,
//                Coordinates = new int[]
//                    {1,2,5,6,7,8,9,10}
//            },
//            new ZipCode()
//            {
//                Zip = 93402,
//                Coordinates = new int[] { 96, 97, 98, 99 }
//            }
//        }.AsQueryable();

//        private IQueryable<Point> _points = new List<Point>()
//        {
//            new Point()
//            {
//                Id = 1,
//                Longitude = -121.12786764474501M,
//                Latitude = 35.594866374176696M
//            },
//            new Point()
//            {
//                Id = 2,
//                Longitude =  -121.125593M,
//                Latitude = 35.595427M
//            },
//            new Point()
//            {
//                Id = 5,
//                Longitude = -121.12764199999998M,
//                Latitude = 35.598861M
//            },
//            new Point()
//            {
//                Id = 6,
//                Longitude = -121.12243599999998M,
//                Latitude = 35.600187M
//            },
//            new Point()
//            {
//                Id = 7,
//                Longitude = -121.123989M,
//                Latitude = 35.611579M
//            },
//            new Point()
//            {
//                Id = 8,
//                Longitude = -121.12178899999999M,
//                Latitude = 35.613671M
//            },
//            new Point()
//            {
//                Id = 9,
//                Longitude = -121.11732799999999M,
//                Latitude = 35.614964M
//            },
//            new Point()
//            {
//                Id = 10,
//                Longitude = -121.11427100000002M,
//                Latitude = 35.619727M
//            },
//            new Point()
//            {
//                Id = 96,
//                Longitude = -120.86240469202802M,
//                Latitude = 35.353866164443794M
//            },
//            new Point()
//            {
//                Id = 97,
//                Longitude = -120.84443899999998M,
//                Latitude = 35.350483M
//            },
//            new Point()
//            {
//                Id = 98,
//                Longitude = -120.844605M,
//                Latitude = 35.346331M
//            },
//            new Point()
//            {
//                Id = 99,
//                Longitude = -120.839849M,
//                Latitude = 35.345361M
//            }
//        }.AsQueryable();


//        [TestMethod]
//        public void TestGetPolygonFeatureCollection()
//        {
//            var zipMockSet = new Mock<DbSet<ZipCode>>();
//            zipMockSet.As<IQueryable<ZipCode>>().Setup(m => m.Provider).Returns(_zipCodeData.Provider);
//            zipMockSet.As<IQueryable<ZipCode>>().Setup(m => m.Expression).Returns(_zipCodeData.Expression);
//            zipMockSet.As<IQueryable<ZipCode>>().Setup(m => m.ElementType).Returns(_zipCodeData.ElementType);
//            zipMockSet.As<IQueryable<ZipCode>>().Setup(m => m.GetEnumerator()).Returns(_zipCodeData.GetEnumerator());

//            var pointMockSet = new Mock<DbSet<Point>>();
//            pointMockSet.As<IQueryable<Point>>().Setup(m => m.Provider).Returns(_points.Provider);
//            pointMockSet.As<IQueryable<Point>>().Setup(m => m.Expression).Returns(_points.Expression);
//            pointMockSet.As<IQueryable<Point>>().Setup(m => m.ElementType).Returns(_points.ElementType);
//            pointMockSet.As<IQueryable<Point>>().Setup(m => m.GetEnumerator()).Returns(_points.GetEnumerator());

//            var censusDataMockSet = new Mock<DbSet<CensusData>>();
//            censusDataMockSet.As<IQueryable<CensusData>>().Setup(m => m.Provider).Returns(_censusData.Provider);
//            censusDataMockSet.As<IQueryable<CensusData>>().Setup(m => m.Expression).Returns(_censusData.Expression);
//            censusDataMockSet.As<IQueryable<CensusData>>().Setup(m => m.ElementType).Returns(_censusData.ElementType);
//            censusDataMockSet.As<IQueryable<CensusData>>().Setup(m => m.GetEnumerator()).Returns(_censusData.GetEnumerator());

//            var censusVariablesMockSet = new Mock<DbSet<CensusVariables>>();
//            censusVariablesMockSet.As<IQueryable<CensusVariables>>().Setup(m => m.Provider).Returns(_census.Provider);
//            censusVariablesMockSet.As<IQueryable<CensusVariables>>().Setup(m => m.Expression).Returns(_census.Expression);
//            censusVariablesMockSet.As<IQueryable<CensusVariables>>().Setup(m => m.ElementType).Returns(_census.ElementType);
//            censusVariablesMockSet.As<IQueryable<CensusVariables>>().Setup(m => m.GetEnumerator()).Returns(_census.GetEnumerator());

//            var mockContext = new Mock<HourglassContext>();
//            mockContext.Setup(m => m.ZipCode).Returns(zipMockSet.Object);
//            mockContext.Setup(m => m.Point).Returns(pointMockSet.Object);
//            mockContext.Setup(m => m.CensusData).Returns(censusDataMockSet.Object);
//            mockContext.Setup(m => m.CensusVariables).Returns(censusVariablesMockSet.Object);

//            var controller = new MapController(mockContext.Object);
//            var results = controller.GetZipCodes("median household income");

//            List<Point> points1 = new List<Point>() {
//                new Point()
//                {
//                    Id = 1,
//                    Longitude = -121.12786764474501M,
//                    Latitude = 35.594866374176696M
//                },
//                new Point()
//                {
//                    Id = 2,
//                    Longitude =  -121.125593M,
//                    Latitude = 35.595427M
//                },
//                new Point()
//                {
//                    Id = 5,
//                    Longitude = -121.12764199999998M,
//                    Latitude = 35.598861M
//                },
//                new Point()
//                {
//                    Id = 6,
//                    Longitude = -121.12243599999998M,
//                    Latitude = 35.600187M
//                },
//                new Point()
//                {
//                    Id = 7,
//                    Longitude = -121.123989M,
//                    Latitude = 35.611579M
//                },
//                new Point()
//                {
//                    Id = 8,
//                    Longitude = -121.12178899999999M,
//                    Latitude = 35.613671M
//                },
//                new Point()
//                {
//                    Id = 9,
//                    Longitude = -121.11732799999999M,
//                    Latitude = 35.614964M
//                },
//                new Point()
//                {
//                    Id = 10,
//                    Longitude = -121.11427100000002M,
//                    Latitude = 35.619727M
//                } };

//            List<Point> points2 = new List<Point>()
//            {
//                new Point()
//                {
//                    Id = 96,
//                    Longitude = -120.86240469202802M,
//                    Latitude = 35.353866164443794M
//                },
//                new Point()
//                {
//                    Id = 97,
//                    Longitude = -120.84443899999998M,
//                    Latitude = 35.350483M
//                },
//                new Point()
//                {
//                    Id = 98,
//                    Longitude = -120.844605M,
//                    Latitude = 35.346331M
//                },
//                new Point()
//                {
//                    Id = 99,
//                    Longitude = -120.839849M,
//                    Latitude = 35.345361M
//                }
//            };

//            PolygonFeature geom1 = new PolygonFeature(points1, "93428");
//            PolygonFeature geom2 = new PolygonFeature(points2, "93402");
//            List<PolygonFeature> features = new List<PolygonFeature>() { geom1, geom2 };
//            PolygonFeatureCollection expectedCollection = new PolygonFeatureCollection(features);
//            //Assert.AreEqual(results.FeatureList.Count(), 2);
//            Console.WriteLine("expected:");
//            Console.WriteLine(expectedCollection.ToString());
//            Assert.AreEqual(results.ToString(), expectedCollection.ToString());
//        }

//        //[TestMethod]
//        //public void TestGetPoint()
//        //{
//        //    var locationMockSet = new Mock<DbSet<Location>>();
//        //    locationMockSet.As<IQueryable<Location>>().Setup(m => m.Provider).Returns(_data.Provider);
//        //    locationMockSet.As<IQueryable<Location>>().Setup(m => m.Expression).Returns(_data.Expression);
//        //    locationMockSet.As<IQueryable<Location>>().Setup(m => m.ElementType).Returns(_data.ElementType);
//        //    locationMockSet.As<IQueryable<Location>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

//        //    var pointMockSet = new Mock<DbSet<Point>>();
//        //    pointMockSet.As<IQueryable<Point>>().Setup(m => m.Provider).Returns(_pdata.Provider);
//        //    pointMockSet.As<IQueryable<Point>>().Setup(m => m.Expression).Returns(_pdata.Expression);
//        //    pointMockSet.As<IQueryable<Point>>().Setup(m => m.ElementType).Returns(_pdata.ElementType);
//        //    pointMockSet.As<IQueryable<Point>>().Setup(m => m.GetEnumerator()).Returns(_pdata.GetEnumerator());

//        //    var mockContext = new Mock<HourglassContext>();
//        //    mockContext.Setup(m => m.Location).Returns(locationMockSet.Object);
//        //    mockContext.Setup(m => m.Point).Returns(pointMockSet.Object);

//        //    var controller = new HourglassServer.MapController(mockContext.Object);
//        //    var results = controller.GetPoint("Colleges");

//        //    PointGeometry expect = new PointGeometry((decimal) - 120.7401, (decimal)35.3292);
//        //    Assert.AreEqual(expect, results);
//        //}

//        //[TestMethod]
//        //public void TestGetFeatures()
//        //{
//        //    var mockSet = new Mock<DbSet<Location>>();
//        //    mockSet.As<IQueryable<Location>>().Setup(m => m.Provider).Returns(_data.Provider);
//        //    mockSet.As<IQueryable<Location>>().Setup(m => m.Expression).Returns(_data.Expression);
//        //    mockSet.As<IQueryable<Location>>().Setup(m => m.ElementType).Returns(_data.ElementType);
//        //    mockSet.As<IQueryable<Location>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

//        //    var mockContext = new Mock<HourglassContext>();
//        //    mockContext.Setup(m => m.Location).Returns(mockSet.Object);

//        //    var controller = new HourglassServer.MapController(mockContext.Object);
//        //    var results = controller.GetFeatures("Colleges");

//        //    List<Feature> expectedList = new List<Feature>()
//        //    {
//        //        new Feature(new Location()
//        //        {
//        //            Name = "Cuesta College",
//        //            TableName = "Colleges",
//        //            Lng = (float)-120.7401,
//        //            Lat = (float)35.3292,
//        //            TableNameNavigation = new Datasetmetadata()
//        //            {
//        //                Tablename = "Colleges",
//        //                Columnnames = "name, latititude, longitude",
//        //                Datatypes = "string, float, float",
//        //                Locationvalues = new bool[] { false, true, true }
//        //            }
//        //        }),
//        //        new Feature(new Location()
//        //            {
//        //                Name = "Cal Poly SLO",
//        //                TableName = "Colleges",
//        //                Lng = (float)-120.6625,
//        //                Lat = (float)35.305,
//        //                TableNameNavigation = new Datasetmetadata()
//        //                {
//        //                    Tablename = "Colleges",
//        //                    Columnnames = "name, latititude, longitude",
//        //                    Datatypes = "string, float, float",
//        //                    Locationvalues = new bool[] { false, true, true }
//        //                }
//        //            }) };

//        //    Assert.AreEqual(results.Name, "Colleges");
//        //    Assert.AreEqual(results.Type, "FeatureCollection");
//        //    Assert.AreEqual(results.FeatureList.Count(), 2);
//        //    Assert.AreEqual(results.FeatureList.First().First().ToString(), expectedList.First().ToString());
//        //}
//    }
//}