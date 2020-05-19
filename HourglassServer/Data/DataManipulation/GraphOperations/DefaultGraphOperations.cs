using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using System.Collections.Generic;
using HourglassServer.Data.Application.GraphModel;

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

        public static async Task<List<GraphApplicationModel>> GetDefaultGraphsModelByCategory(HourglassContext db ,string category){
            List<GraphApplicationModel> graphs = new List<GraphApplicationModel>();
            List <string> defaultsIds = await db.DefaultGraph
                    .Where(g => g.Category.Equals(category))
                    .Select(g => g.GraphId) 
                    .ToListAsync();
            foreach (string id in defaultsIds){
                GraphApplicationModel graph = await GraphModelRetriever.GetGraphApplicationModelById(db, id);
                graphs.Add(graph);
            }
            return graphs;
        }
    }
}