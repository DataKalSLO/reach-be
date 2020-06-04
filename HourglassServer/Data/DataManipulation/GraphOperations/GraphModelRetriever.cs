using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelRetriever
    {
        public static async Task<GraphApplicationModel> GetGraphApplicationModelById(HourglassContext db, string graphId)
        {
            Graph requestedGraph = await db.Graph
                    .Include(g => g.GraphSource)
                    .Include(g => g.User)
                    .SingleAsync(g => g.GraphId == graphId);

            return GraphFactory.CreateGraphApplicationModel(requestedGraph, requestedGraph.GraphSource.ToArray());
        }

        public static async Task<List<GraphApplicationModel>> GetGraphApplictionModelsforUser(HourglassContext db, string userId)
        {
            List<GraphApplicationModel> graphModels = new List<GraphApplicationModel>();
            List<Graph> graphsForUser = await db.Graph
                    .Where(g => g.UserId == userId)
                    .Include(g => g.GraphSource)
                    .Include(g => g.User)
                    .ToListAsync();
            foreach (Graph graph in graphsForUser)
            {
                GraphApplicationModel graphModel =
                    GraphFactory.CreateGraphApplicationModel(graph, graph.GraphSource.ToArray());
                graphModels.Add(graphModel);
            }
            return graphModels;
        }
    }
}
