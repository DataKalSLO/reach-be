using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelDeleter
    {
        public static async Task DeleteGraphById(HourglassContext db, string graphId, string currentUserId)
        {
            Graph graphToDelete = await db.FindAsync<Graph>(graphId);

            if (graphToDelete == null)
            {
                throw new ItemNotFoundException(
                    message: String.Format("No graph found with id {0}. ", graphId),
                    tag: "NotFound"
                );
            }

            if (graphToDelete.UserId != currentUserId)
            {
                throw new PermissionDeniedException(
                    message: String.Format("Unable to delete. {0} is not the owner of graph {1}.", currentUserId, graphId),
                    tag: "PermissionDenied"
                );
            }

            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.DELETE, graphToDelete);
            await db.SaveChangesAsync();
        }

        public static void PerformDeleteOperationForGraphSources(HourglassContext db, GraphSource[] sources)
        {
            foreach (GraphSource source in sources)
            {
                DbSetMutator.PerformOperationOnDbSet<GraphSource>(
                    db.GraphSource,
                    MutatorOperations.DELETE,
                    source
                );
            }
        }
    }
}
