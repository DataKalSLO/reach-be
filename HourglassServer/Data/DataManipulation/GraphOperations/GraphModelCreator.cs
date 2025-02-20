using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelCreator
    {
        public static async Task<GraphApplicationModel> CreateGraph(HourglassContext db,
                IConfiguration config, GraphModel graphModel, string currentUserId)
        {
            // Append the userId from the session token to the graph model
            graphModel.UserId = currentUserId;

            // Generate a new graphId and append it to the graph model
            graphModel.GraphId = GraphFactory.GenerateNewGraphId();

            Graph graph = GraphFactory.CreateGraphFromGraphModel(graphModel);
            GraphSource[] sources = GraphFactory.CreateGraphSourcesFromGraphSourceModel
            (
                graphModel.DataSources,
                graph.GraphId
            );

            graph.SnapshotUrl = await GraphSnapshotOperations.UploadSnapshotToS3(config, graphModel);

            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.ADD, graph);
            GraphSourceOperations.PerformOperationForGraphSources(db, MutatorOperations.ADD, sources);
            await db.SaveChangesAsync();

            return GraphFactory.CreateGraphApplicationModel(graph, sources);
        }
    }
}
