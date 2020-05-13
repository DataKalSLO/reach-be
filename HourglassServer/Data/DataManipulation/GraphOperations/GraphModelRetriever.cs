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

            GraphSourceModel[] sources = GraphSourceModel.convertPersistentGraphSource(requestedGraph.GraphSource.ToArray());

            return new GraphApplicationModel
            {
                GraphId = requestedGraph.GraphId,
                UserId = requestedGraph.UserId,
                TimeStamp = requestedGraph.Timestamp.Value,
                GraphTitle = requestedGraph.GraphTitle,
                SnapshotUrl = requestedGraph.SnapshotUrl,
                DataSources = sources,
                GraphOptions = requestedGraph.GraphOptions
            };
        }
    }
}
