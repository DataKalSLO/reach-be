using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServer.Data.DataManipulation.GraphOperations;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {
        private readonly HourglassContext _context;
        public GraphController(HourglassContext context)
        {
            _context = context;
        }

        [Route("getDefaultGraphs/{category}")]
        [HttpGet]
        public async Task<IActionResult> getDefaultGraphs(string category)
        {
            List<GraphApplicationModel> defaults = 
                await DefaultGraphOperations.GetDefaultGraphsModelByCategory(this._context, category);

            return new OkObjectResult(defaults);
             
        }

        [UserExists]
        [HttpGet]
        public async Task<IActionResult> getGraphsforUser()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Single().Value;
                List<GraphApplicationModel> graph = 
                    await GraphModelRetriever.GetGraphApplictionModelsforUser(this._context, currentUserId);
                return new OkObjectResult(graph);
            }
            catch (Exception e)
            {
                return BadRequest(
                    new HourglassError(e.ToString(), "User Error")
                );
            }
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

                // Add to the default graph table if administrator requests
                if (HttpContext.User.HasRole(Role.Admin) && graphModel.GraphCategory != null)
                {
                    await DefaultGraphOperations.PerformOperationForDefaultGraph(
                        this._context,
                        MutatorOperations.ADD,
                        graph.GraphId,
                        graphModel.GraphCategory);
                }

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

                // Update the default graph table if administrator requests
                if (HttpContext.User.HasRole(Role.Admin) && graphModel.GraphCategory != null)
                {
                    await DefaultGraphOperations.PerformOperationForDefaultGraph(
                        this._context,
                        MutatorOperations.UPDATE,
                        graph.GraphId,
                        graphModel.GraphCategory);
                }

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
