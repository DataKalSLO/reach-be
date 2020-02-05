using HourglassServer.Data;
using HourglassServer.EndpointResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HourglassServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private CentralToastContext _context;

        public EducationController(CentralToastContext context)
        {
            _context = context;
        }

        [HttpGet("universities")]
        public string Get()
        {
            using (var sr = new StreamReader("Data/college_data.json"))
            {
                return sr.ReadToEnd();
            }
        }

        [HttpGet("degrees")]
        public IEnumerable<XYGraphData> Get(string university)
        {
            return (from degrees in _context.Degrees
                    where degrees.University == university
                    group degrees by degrees.Year into groupedDegrees
                    select new XYGraphData
                    {
                        X = groupedDegrees.Key,
                        Y = groupedDegrees.Sum(degreesSameYear => degreesSameYear.Completions).GetValueOrDefault(0)
                    });
        }
    }
}