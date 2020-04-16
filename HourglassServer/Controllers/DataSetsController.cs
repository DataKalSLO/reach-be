using System;
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
        public ActionResult<DataSet> GetDataSet(string tableName) {
            try {
                return _context.getDataSet(tableName).Result;
            }
            
            // Note: when using Task.Result on a task that faults, the exception that caused the
            // Task to fault is propagated, but it’s not thrown directly. Instead, it's wrapped 
            // in an AggregateException object
            catch (AggregateException exceptions) {
                var error = "";
                var message = "";

                foreach(var e in exceptions.Flatten().InnerExceptions) {
                    if (e is TableNotFoundException) {
                        error = "Bad Request";
                        message = string.Format("The provided table name does not exist: {0}", tableName);
                        break;
                    }
                    if (e is StaleRequestException) {
                        error = "Stale Request";
                        message = string.Format("Table no longer exists in database: {0}", tableName);
                        break;
                    }
                }

                return BadRequest(
                    new ExceptionMessageContent() { 
                            Error = error, 
                            Message = message
                    }
                );
            } 
        }
    }
} 
