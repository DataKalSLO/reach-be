using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphSourceOperations
    {
        public static void PerformOperationForGraphSources(
                HourglassContext db,
                MutatorOperations mutatorOperation,
                GraphSource[] sources)
        {
            foreach (GraphSource source in sources)
            {
                DbSetMutator.PerformOperationOnDbSet<GraphSource>(
                    db.GraphSource,
                    mutatorOperation,
                    source
                );
            }
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