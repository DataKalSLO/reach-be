﻿using HourglassServer.Custom.Upload;
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
           await _context.InsertAsync(uploadData.MeanRealWagesAdjColSlo);
           return Ok(new { rowsInserted = uploadData.MeanRealWagesAdjColSlo.Count() });
        }
        
        [HttpPost("incomeInequalitySlo")]
        public async Task<IActionResult> Post([FromBody] IncomeInequalitySloUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.IncomeInequalitySlo);
            return Ok(new { rowsInserted = uploadData.IncomeInequalitySlo.Count() });
        }

        [HttpPost("netMigrationSlo")]
        public async Task<IActionResult> Post([FromBody] NetMigrationSloUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.NetMigrationSlo);
            return Ok(new { rowsInserted = uploadData.NetMigrationSlo.Count() });
        }

        [HttpPost("sloAirports")]
        public async Task<IActionResult> Post([FromBody] SloAirportsUploadModel uploadData)
        {
            await _context.InsertAsync(uploadData.SloAirports);
            return Ok(new { rowsInserted = uploadData.SloAirports.Count() });
        }

        [HttpPost("universityInfo")]
        public async Task<IActionResult> Post([FromBody] UniversityInfoUploadModel uploadData)
        {
          await _context.InsertAsync(uploadData.UniversityInfo);
          return Ok(new { rowsInserted = uploadData.UniversityInfo.Count() });
        }
   }
}
