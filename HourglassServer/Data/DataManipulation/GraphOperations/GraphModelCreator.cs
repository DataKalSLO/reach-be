using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelCreator
    {
        public static async Task<GraphApplicationModel> CreateGraph(HourglassContext db, GraphModel graphModel, string currentUserId)
        {
            // Append the userId from the session token to the graph model
            graphModel.UserId = currentUserId;

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

            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.ADD, graph);
            GraphSourceOperations.PerformOperationForGraphSources(db, MutatorOperations.ADD, sources);
            await db.SaveChangesAsync();

            return GraphFactory.CreateGraphApplicationModel(graph, sources);
        }
    }
}
