using HourglassServer.Data;
using System.Threading.Tasks;
//using HourglassServer.EndpointResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using System.IO;
//using System.Linq;
//using Npgsql;

/**
 *----------------------------------------
 * Meta Data Controller class
 *----------------------------------------
 * This class provides access to MetaData API Methods
 * This class provides the following methods:
 * -GetMetaData
 *     -Using the [HttpGet] route endpoint,
 *      this method will return a the MetaData
 *      from for all the datasets as a list
 *      of MetaData Objects 
 *----------------------------------------
 */

namespace HourglassServer.Controllers{

    [DefaultControllerRoute]
    public class MetaDataController : ControllerBase{
        //create a Database context
        private DatasetDbContext _context;

        public MetaDataController(DatasetDbContext context)
        {
            //set DatabaseContext in MetaDataController constructor
            _context = context;
        }

        // GET: api/MetaData
        [HttpGet]
        public ActionResult<List<DatasetMetadata>> GetMetaData(){

            return _context.getAllDatasetMetadata().Result;
        }
    } 
}