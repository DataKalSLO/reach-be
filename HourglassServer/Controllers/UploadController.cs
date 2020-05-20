using HourglassServer.Custom.Upload;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            await _context.InsertAsync(uploadData.CovidUnemployment);
            return Ok(new { tableName = "covid_unemployment"});
        }
    }
}
