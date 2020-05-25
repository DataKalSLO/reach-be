using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using HourglassServer.Data;
using HourglassServer.Data.Application.MetadataModel;
using HourglassServer.Data.DataManipulation.MetadataOperations;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class MetaDataController : Controller
    {
        private HourglassContext _context;

        private IMemoryCache _cache;

        public MetaDataController(HourglassContext context,
                IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetMetaData()
        {
            try
            {
                List<MetadataApplicationModel> metadata = await _cache.GetMetadata(_context);
                return new OkObjectResult(metadata);
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "Error"));
            }
        }
    }
}