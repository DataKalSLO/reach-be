using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HourglassServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HourglassServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        private CentralToastContext _context;

        public DummyController(CentralToastContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string Get()
        {
            return _context.Dummy.Where(d => d.Id == 0).FirstOrDefault().Data;
        }
    }
}