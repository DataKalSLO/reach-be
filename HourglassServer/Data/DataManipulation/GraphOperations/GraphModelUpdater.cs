using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphModelUpdater
    {
        public static async Task<GraphApplicationModel> UpdateGraph(HourglassContext db, GraphModel graphModel, string currentUserId)
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

            await updateGraphInDb(db, updatedGraph);
            await updateGraphSourcesInDb(db, existingGraphSources, updatedGraphSources);

            return new GraphApplicationModel
            {
                GraphId = updatedGraph.GraphId,
                UserId = updatedGraph.UserId,
                TimeStamp = updatedGraph.Timestamp.Value,
                GraphTitle = updatedGraph.GraphTitle,
                SnapshotUrl = updatedGraph.SnapshotUrl,
                DataSources = graphModel.DataSources,
                GraphOptions = updatedGraph.GraphOptions
            };
        }

        public static void PerformUpdateOperationForGraphSources(HourglassContext db, GraphSource[] sources)
        {
            foreach (GraphSource source in sources)
            {
                DbSetMutator.PerformOperationOnDbSet<GraphSource>(
                    db.GraphSource,
                    MutatorOperations.UPDATE,
                    source
                );
            }
        }

        private static async Task updateGraphInDb(HourglassContext db, Graph updatedGraph)
        {
            DbSetMutator.PerformOperationOnDbSet<Graph>(db.Graph, MutatorOperations.UPDATE, updatedGraph);
            await db.SaveChangesAsync();
        }

        // SourcesToAdd = {Updated} set difference {Existing} 
        // SourcesToUpdate = {Updated} set intersect {Existing}
        // SourcesToRemove = {Existing} set difference {Updated}
        private static async Task updateGraphSourcesInDb(
                HourglassContext db,
                List<GraphSource> existingGraphSources,
                List<GraphSource> updatedGraphSources)
        {
            var sourcesToAdd = updatedGraphSources.Except(existingGraphSources, new GraphSourceSeriesComparor()).ToArray();
            var sourcesToUpdate = updatedGraphSources.Intersect(existingGraphSources, new GraphSourceSeriesComparor()).ToArray();
            var sourcesToRemove = existingGraphSources.Except(updatedGraphSources, new GraphSourceSeriesComparor()).ToArray();

            GraphModelCreator.PerformAddOperationForGraphSources(db, sourcesToAdd);
            GraphModelUpdater.PerformUpdateOperationForGraphSources(db, sourcesToUpdate);
            GraphModelDeleter.PerformDeleteOperationForGraphSources(db, sourcesToRemove);

            await db.SaveChangesAsync();
        }
    }

    // Comparor object for comparing two graph series.
    public class GraphSourceSeriesComparor : IEqualityComparer<GraphSource>
    {
        // Graph sources are considered equal if they share the same series type
        public bool Equals(GraphSource x, GraphSource y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the series types are the same
            return x.SeriesType == y.SeriesType;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(GraphSource source)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(source, null)) return 0;

            //Get hash code for the SeriesType field if it is not null.
            return source.SeriesType == null ? 0 : source.SeriesType.GetHashCode();
        }
    }
}
