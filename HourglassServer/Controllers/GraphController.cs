using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {
        private DatasetDbContext _context;
        public GraphController(DatasetDbContext context)
        {
            //set DatabaseContext in DataSetsController constructor
            _context = context;
        }

<<<<<<< HEAD
        [HttpGet("{category}")]
        public ActionResult<List<storedGraph>> getDefaultGraphs(string category){
            return _context.getDefultGraphs(category).Result;
=======
        [Route("getDefaultGraphs/{category}")]
        [HttpGet]
        public async Task<IActionResult> getDefaultGraphs(string category)
        {
            List<GraphApplicationModel> defaults =
                await DefaultGraphOperations.GetDefaultGraphsModelByCategory(this._context, category);

            return new OkObjectResult(defaults);
>>>>>>> 843b14620618988334c2f2284e79384818941d71
        }
        // GET api/<controller>/5
        [HttpGet]
        public string Get()
        {
            return "Retrieving graphs not yet implemented";
        }

        // POST api/<controller>
        [HttpPost]
        public string Post()
        {
            return "Creating Graphs is not yet implemented";
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating Graphs is not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete()
        {
<<<<<<< HEAD
            return "Deleting Graphs is not yet implemented";
=======
            try
            {
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
                await GraphModelDeleter.DeleteGraphById(_context, graphId, currentUserId);

                return new OkObjectResult(new { graphId });
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.errorObj);
            }
            catch (PermissionDeniedException e)
            {
                return BadRequest(e.errorObj);
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "UnknownError"));
            }
>>>>>>> 843b14620618988334c2f2284e79384818941d71
        }
    }
}
