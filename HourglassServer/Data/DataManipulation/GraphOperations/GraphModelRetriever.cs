using System.Linq;
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
                    .SingleAsync(g => g.GraphId == graphId);

            return GraphFactory.CreateGraphApplicationModel(requestedGraph, requestedGraph.GraphSource.ToArray());
        }
    }
}
