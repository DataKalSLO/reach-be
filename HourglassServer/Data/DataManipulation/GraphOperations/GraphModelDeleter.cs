using System;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelDeleter
    {
        public static async Task DeleteGraphById(HourglassContext db,
                IConfiguration config, string graphId, string currentUserId)
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

            await GraphSnapshotOperations.RemoveSnapshotFromS3(config, graphId);

            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.DELETE, graphToDelete);
            await db.SaveChangesAsync();
        }
    }
}
