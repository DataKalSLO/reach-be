using Microsoft.AspNetCore.Mvc;
using System;
using HourglassServer.Custom.Upload;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;

namespace HourglassServer
{
    [DefaultControllerRoute]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private HourglassContext _context;

        public UploadController(HourglassContext context)
        {
            _context = context;
        }

        [HttpPost("covid_unemployment")]
        public async Task<IActionResult> Post([FromBody] CovidUnemploymentUploadModel uploadData)
        {
            await _context.InsertAsync(
                uploadData.CovidUnemploymentUploadModel
            );

            return Ok(new { tableName = "covid_unemployment"});
        }
    }
}
