using HourglassServer.Data;
using System.Threading.Tasks;
//using HourglassServer.EndpointResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;

/**
 *----------------------------------------
 * Data Sets Controller class 
 *----------------------------------------
 * This class provides access to DataSets API Methods
 * This class provides the following methods:
 * -GetDataSet
 *     -Using the [HttpGet] route endpoint,
 *      this method will return a the rquested
 *      data set from the database using the 
 *      provided database context
 *----------------------------------------
 */

namespace HourglassServer.Controllers{
    [DefaultControllerRoute]
    public class DataSetsController : ControllerBase{
        private DatasetDbContext _context;
        public DataSetsController(DatasetDbContext context)
        {
            //set DatabaseContext in DataSetsController constructor
            _context = context;
        }

        // GET: api/DataSets?tableName=[string tableName]
        [HttpGet]
        public ActionResult<DataSet> GetDataSet(string tableName){

            return _context.getDataSet(tableName).Result;         
        }
    }
} 
