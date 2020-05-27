using HourglassServer.Custom.Upload;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

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


        [HttpPost("airports")]
        public async Task<IActionResult> Post([FromBody] AirportsUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.Airports);
            return Ok(new { rowsInserted = uploadData.Airports.Count() });
        }

        [HttpPost("commuteTimes")]
        public async Task<IActionResult> Post([FromBody] CommuteTimesUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.CommuteTimes);
            return Ok(new { rowsInserted = uploadData.CommuteTimes.Count() });
        }

        [HttpPost("covidUnemployment")]
        public async Task<IActionResult> Post([FromBody] CovidUnemploymentUploadModel uploadData)
        {
           await _context.InsertAsync(uploadData.CovidUnemployment);
           return Ok(new { rowsInserted = uploadData.CovidUnemployment.Count() });
        }
   }
}
