using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelUpdater
    {
        public static async Task<GraphApplicationModel> UpdateGraph(HourglassContext db,
                IConfiguration config, GraphModel graphModel, string currentUserId)
        {
            string graphOwnerId = await db.Graph
                .Where(g => g.GraphId == graphModel.GraphId)
                .Select(g => g.UserId)
                .SingleAsync();

            if (graphOwnerId != currentUserId)
            {
                throw new PermissionDeniedException(
                    message: String.Format("Unable to modify. {0} is not the owner of graph {1}.",
                                currentUserId,
                                graphModel.GraphId),
                    tag: "PermissionDenied"
                );
            }

            // Append the userId from the session token to the graph model
            graphModel.UserId = currentUserId;

            Graph updatedGraph = GraphFactory.CreateGraphFromGraphModel(graphModel);

            // Create a list of the updated graph sources from the request
            List<GraphSource> updatedGraphSources = GraphFactory.CreateGraphSourcesFromGraphSourceModel
            (
                graphModel.DataSources,
                graphModel.GraphId
            ).ToList();

            // Get a list of the existing graph sources associated with this graph from the database
            List<GraphSource> existingGraphSources = await db.GraphSource
                .Where(gs => gs.GraphId == graphModel.GraphId)
                .AsNoTracking()
                .ToListAsync();

            await GraphSnapshotOperations.RemoveSnapshotFromS3(config, graphModel.GraphId);
            updatedGraph.SnapshotUrl = await GraphSnapshotOperations.UploadSnapshotToS3(config, graphModel);

            // Perform update to the graph in the DB
            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.UPDATE, updatedGraph);
            await db.SaveChangesAsync();

            await UpdateGraphSourcesInDb(db, existingGraphSources, updatedGraphSources);

            return GraphFactory.CreateGraphApplicationModel(updatedGraph, updatedGraphSources.ToArray());
        }

        // SourcesToAdd = {Updated} set difference {Existing} 
        // SourcesToUpdate = {Updated} set intersect {Existing}
        // SourcesToRemove = {Existing} set difference {Updated}
        private static async Task UpdateGraphSourcesInDb(
                HourglassContext db,
                List<GraphSource> existingGraphSources,
                List<GraphSource> updatedGraphSources)
        {
            var sourcesToAdd = updatedGraphSources.Except(existingGraphSources, new GraphSourceSeriesComparor()).ToArray();
            var sourcesToUpdate = updatedGraphSources.Intersect(existingGraphSources, new GraphSourceSeriesComparor()).ToArray();
            var sourcesToRemove = existingGraphSources.Except(updatedGraphSources, new GraphSourceSeriesComparor()).ToArray();

            GraphSourceOperations.PerformOperationForGraphSources(db, MutatorOperations.ADD, sourcesToAdd);
            GraphSourceOperations.PerformOperationForGraphSources(db, MutatorOperations.UPDATE, sourcesToUpdate);
            GraphSourceOperations.PerformOperationForGraphSources(db, MutatorOperations.DELETE, sourcesToRemove);

            await db.SaveChangesAsync();
        }
    }
}
