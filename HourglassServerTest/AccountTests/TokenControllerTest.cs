using System.Linq;
using System;
using Moq; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using Microsoft.EntityFrameworkCore;
using HourglassServer.Data.DataManipulation.StoryModel;
using System.Collections.Generic;
using HourglassServer.Controllers;
using Microsoft.Extensions.Configuration;
using HourglassServerTest.StoryTests;

namespace HourglassServerTest.AccountTests
{
    [TestClass]
    public class TokenControllerTest
    {
        public HourglassContext context;
        public HourglassServer.TokenModel token;
        public Mock<HourglassContext> MockContext;
        public Mock<DbSet<Person>> PersonDbSet;
        private List<Person> _data;

        private static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet;
        }

        [TestInitialize]
        public void InitTest()
        {
            _data = new List<Person>()
            {
                new Person()
                {
                    Email = "test@test.com",
                    Name = "test",
                    Role = 0,
                    Salt = "salt",
                    PasswordHash = "hash",
                    Occupation = "policymaker",
                    NotificationsEnabled = true
                }
            };
            PersonDbSet = GetQueryableMockDbSet(_data);
            MockContext = new Mock<HourglassContext>();
            MockContext.Setup(m => m.Person).Returns(PersonDbSet.Object);
            context = MockContext.Object;
            token = new HourglassServer.TokenModel()
            {
                Email = "test@test.com",
                Password = "test12!"
            };
            
        }

        [TestMethod]
        public void testUnauthorizedUser()
        {
            Assert.AreEqual(HourglassServer.TokenController.isUnauthorizedUser(token, context), true);
        }
    }
}