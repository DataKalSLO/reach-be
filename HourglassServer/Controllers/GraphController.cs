using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.GraphOperations;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServer.Models.Persistent;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        [HttpGet("{graphId}")]
        public IActionResult GetGraphById(string graphId)
        {
            try
            {
                Graph requestedGraph = _context.Graph.Include(g => g.GraphSource).Single(g => g.GraphId == graphId);
                GraphSourceModel[] sources = GraphSourceModel.convertGraphSources(requestedGraph.GraphSource.ToArray());
                
                return new OkObjectResult(new GraphResponseModel {
                    GraphId = requestedGraph.GraphId,
                    UserId = requestedGraph.UserId,
                    TimeStamp = requestedGraph.Timestamp.Value,
                    GraphTitle = requestedGraph.GraphTitle,
                    SnapshotUrl = requestedGraph.SnapshotUrl,
                    DataSources = sources,
                    GraphOptions = requestedGraph.GraphOptions
                });
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
        public IActionResult CreateGraph([FromBody] GraphModel graphModel)
        {
            try 
            {
                // Append the userId from the session token to the graph model
                graphModel.UserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;

                // Generate a new graphId and append it to the graph model
                graphModel.GraphId = GraphFactory.GenerateNewGraphId();

                // TODO: If category is passed with the request and the user is an adminstrator, add the
                // graphId and category to the DefaultGraphs table

                Graph graph = GraphFactory.CreateGraphFromGraphModel(graphModel);
                GraphSource[] sources = GraphFactory.CreateGraphSourcesFromGraphSourceModel
                (
                    graphModel.DataSources, 
                    graph.GraphId
                );

                DbSetMutator.PerformOperationOnDbSet<Graph>(_context.Graph, MutatorOperations.ADD, graph);

                foreach (GraphSource source in sources) {
                    DbSetMutator.PerformOperationOnDbSet<GraphSource>(
                        _context.GraphSource, 
                        MutatorOperations.ADD, 
                        source
                    );
                }

                _context.SaveChanges();
                
                return new OkObjectResult(new GraphResponseModel {
                    GraphId = graph.GraphId,
                    UserId = graph.UserId,
                    TimeStamp = graph.Timestamp.Value,
                    GraphTitle = graph.GraphTitle,
                    SnapshotUrl = graph.SnapshotUrl,
                    DataSources = graphModel.DataSources,
                    GraphOptions = graph.GraphOptions
                });
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "Error"));
            }
        }

        [UserExists]
        [HttpPut]
        public IActionResult Put([FromBody] GraphModel graphModel)
        {
            try 
            {
                Graph graphToModify = _context.Graph.Single(g => g.GraphId == graphModel.GraphId);

                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;

                if (graphToModify.UserId != currentUserId)
                {
                    return BadRequest(
                        new HourglassError(
                            String.Format("Unable to modify. {0} is not the owner of graph {1}.", 
                                currentUserId, 
                                graphModel.GraphId), 
                            "PermissionDenied")
                    );
                }

                // Append the userId from the session token to the graph model
                graphModel.UserId = currentUserId;
                
                Graph updatedGraph = GraphFactory.CreateGraphFromGraphModel(graphModel);

                _context.Entry(graphToModify).State = EntityState.Detached;
                DbSetMutator.PerformOperationOnDbSet<Graph>(_context.Graph, MutatorOperations.UPDATE, updatedGraph);
                _context.SaveChanges();

                // Create a list of the updated graph sources
                List<GraphSource> updatedSources = GraphFactory.CreateGraphSourcesFromGraphSourceModel
                (
                    graphModel.DataSources, 
                    graphModel.GraphId
                ).ToList();

                // Get a list of the existing graph sources associated with this graph from the database
                List<GraphSource> existingGraphSources = _context.GraphSource
                    .Where(gs => gs.GraphId == graphModel.GraphId)
                    .AsNoTracking()
                    .ToList();

                // Delete all existing GraphSource that are not being updated (for example, 
                // if a series is dropped from the graph with the update)
                var updatedSourcesSeriesTypes = updatedSources.Select(s => s.SeriesType).ToList();
                foreach (GraphSource oldSource in existingGraphSources) {
                    if (!updatedSourcesSeriesTypes.Contains(oldSource.SeriesType)) 
                    {
                        DbSetMutator.PerformOperationOnDbSet<GraphSource>
                        (
                            _context.GraphSource, 
                            MutatorOperations.DELETE, 
                            oldSource
                        );
                    }
                    else
                    {
                        // Perform the update for the series if it has not been dropped
                        GraphSource updateSource = updatedSources.Single(s => s.SeriesType == oldSource.SeriesType);
                        DbSetMutator.PerformOperationOnDbSet<GraphSource>
                        (
                            _context.GraphSource, 
                            MutatorOperations.UPDATE, 
                            updateSource
                        );

                        updatedSources.Remove(updateSource);
                    }
                }

                // Add any remaining new sources (for example, if a new series is being added with the update)
                foreach (GraphSource newSource in updatedSources)
                {
                    DbSetMutator.PerformOperationOnDbSet<GraphSource>
                        (
                            _context.GraphSource, 
                            MutatorOperations.ADD, 
                            newSource
                        );
                }

                _context.SaveChanges();

                return new OkObjectResult(new GraphResponseModel {
                    GraphId = updatedGraph.GraphId,
                    UserId = updatedGraph.UserId,
                    TimeStamp = updatedGraph.Timestamp.Value,
                    GraphTitle = updatedGraph.GraphTitle,
                    SnapshotUrl = updatedGraph.SnapshotUrl,
                    DataSources = graphModel.DataSources,
                    GraphOptions = updatedGraph.GraphOptions
                });
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
        public IActionResult DeleteGraphById(string graphId)
        {
            try
            {
                Graph graphToDelete = _context.Find<Graph>(graphId);
                var currentUserId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;

                if (graphToDelete == null)
                {
                    return BadRequest(
                        new HourglassError(
                            String.Format("No graph found with id {0}. ", graphId), 
                            "NotFound")
                    );
                }

                if (graphToDelete.UserId != currentUserId)
                {
                    return BadRequest(
                        new HourglassError(
                            String.Format("Unable to delete. {0} is not the owner of graph {1}.", currentUserId, graphId), 
                            "PermissionDenied")
                    );
                }

                DbSetMutator.PerformOperationOnDbSet<Graph>(_context.Graph, MutatorOperations.DELETE, graphToDelete);
                _context.SaveChanges();

                return new OkObjectResult(String.Format("Successfully deleted graph with id {0}.", graphId));
            }
            catch (Exception e)
            {
                return BadRequest(new HourglassError(e.ToString(), "Error") );
            }
        }
    }
}
