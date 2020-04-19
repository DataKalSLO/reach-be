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

      
    }
}