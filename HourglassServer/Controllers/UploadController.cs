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

        [HttpPost("covidUnemployment")]
        public async Task<IActionResult> Post([FromBody] CovidUnemploymentUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.CovidUnemployment);
            return Ok(new { rowsInserted = uploadData.CovidUnemployment.Count()});
        }

        [HttpPost("airports")]
        public async Task<IActionResult> Post([FromBody] AirportsUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.Airports);
            return Ok(new { rowsInserted = uploadData.Airports.Count() });
        }

        [HttpPost("meanRealWagesAdjColSlo")]
        public async Task<IActionResult> Post([FromBody] MeanRealWagesAdjColSloUploadModel uploadData)
        {
           await _context.InsertAsync(uploadData.MeanRealWagesAdjColSlos);
           return Ok(new { rowsInserted = uploadData.MeanRealWagesAdjColSlos.Count() });
        }
   }
}
