using HourglassServer.Data;
using HourglassServer.EndpointResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HourglassServer.Controllers;
using System.Linq;
using System.Collections.Generic;
using System;

namespace HourglassServerTest
{
    [TestClass]
    public class EducationControllerTest
    {
        private IQueryable<Degrees> _data = new List<Degrees>()
        {
            new Degrees()
            {
                Gender = "male",
                Year = "2018",
                IdUniversity = "1234",
                University = "Cal Poly",
                Completions = 10000
            },
            new Degrees()
            {
                Gender = "female",
                Year = "2017",
                IdUniversity = "2345",
                University = "UCSB",
                Completions = 5000
            },
            new Degrees()
            {
                Gender = "female",
                Year = "2018",
                IdUniversity = "1234",
                University = "Cal Poly",
                Completions = 15000
            },
            new Degrees()
            {
                Gender = "female",
                Year = "2019",
                IdUniversity = "1234",
                University = "Cal Poly",
                Completions = 1000
            }
        }.AsQueryable();

        [TestMethod]
        public void TestGetDegreesGrouping()
        {
            var mockSet = new Mock<DbSet<Degrees>>();
            mockSet.As<IQueryable<Degrees>>().Setup(m => m.Provider).Returns(_data.Provider);
            mockSet.As<IQueryable<Degrees>>().Setup(m => m.Expression).Returns(_data.Expression);
            mockSet.As<IQueryable<Degrees>>().Setup(m => m.ElementType).Returns(_data.ElementType);
            mockSet.As<IQueryable<Degrees>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

            var mockContext = new Mock<HourglassContext>();
            mockContext.Setup(m => m.Degrees).Returns(mockSet.Object);

            var controller = new EducationController(mockContext.Object);
            var results = controller.Get("Cal Poly").ToList();

            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].X, "2018");
            Assert.AreEqual(results[0].Y, 25000);
            Assert.AreEqual(results[1].X, "2019");
            Assert.AreEqual(results[1].Y, 1000);
        }
    }
}