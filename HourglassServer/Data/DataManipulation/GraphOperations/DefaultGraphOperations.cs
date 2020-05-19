using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class DefaultGraphOperations
    {
        public static async Task PerformOperationForDefaultGraph(
                HourglassContext db,
                MutatorOperations operation,
                string graphId,
                string category)
        {
            DefaultGraph defaultGraph = GraphFactory.CreateDefaultGraph(graphId, category);
            DbSetMutator.PerformOperationOnDbSet<DefaultGraph>(db.DefaultGraph, operation, defaultGraph);
            await db.SaveChangesAsync();
        }
    }
}