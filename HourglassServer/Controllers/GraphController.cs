using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.GraphOperations;
using System.Collections.Generic;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {
        private readonly HourglassContext _context;
        private DatasetDbContext _dbContext;

        public GraphController(HourglassContext context, DatasetDbContext dbContext)
        {
            _context = context;
            _dbContext = dbContext;
        }

        [Route("getDefaultGraphs/{category}")]
        [HttpGet]
        public ActionResult<List<storedGraph>> getDefaultGraphs(string category)
        {
            return _dbContext.getDefultGraphs(category).Result;
        }

        [HttpGet("{graphId}")]
        public async Task<IActionResult> GetGraphById(string graphId)
        {
            try
            {
                GraphApplicationModel graph = await GraphModelRetriever.GetGraphApplicationModelById(this._context, graphId);
                return new OkObjectResult(graph);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(
                    new HourglassError(
                        String.Format("No graph found with id {0}. ", graphId),
                        "NotFound")
                );
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "Error"));
            }
        }

        [UserExists]
        [HttpPost]
        public async Task<IActionResult> CreateGraph([FromBody] GraphModel graphModel)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
                GraphApplicationModel graph = await GraphModelCreator.CreateGraph(this._context, graphModel, currentUserId);
                return new OkObjectResult(graph);
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "UnknownError"));
            }
        }

        [UserExists]
        [HttpPut]
        public async Task<IActionResult> UpdateGraph([FromBody] GraphModel graphModel)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
                GraphApplicationModel graph = await GraphModelUpdater.UpdateGraph(this._context, graphModel, currentUserId);
                return new OkObjectResult(graph);
            }
            catch (PermissionDeniedException e)
            {
                return BadRequest(e.errorObj);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(
                    new HourglassError(
                        String.Format("No graph found with id {0}. ", graphModel.GraphId),
                        "NotFound")
                );
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "Error"));
            }
        }

        [UserExists]
        [HttpDelete("{graphId}")]
        public async Task<IActionResult> DeleteGraphById(string graphId)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
                await GraphModelDeleter.DeleteGraphById(_context, graphId, currentUserId);
                return new OkObjectResult(String.Format("Successfully deleted graph with id {0}.", graphId));
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
        }
    }
}
