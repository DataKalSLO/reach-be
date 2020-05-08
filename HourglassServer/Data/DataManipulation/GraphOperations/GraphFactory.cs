using System;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphFactory
    {
        public static Graph CreateGraphFromGraphModel(GraphModel model) 
        {
            var guid = Guid.NewGuid().ToString();
            var graphOptions = model.GraphOptions.ToString();

            return new Graph() 
            {
                GraphId = guid,
                GraphTitle = model.GraphTitle,
                UserId = model.UserId,
                Timestamp = model.Timestamp,
                SnapshotUrl = "NOT_IMPLEMENTED",
                GraphOptions = graphOptions
            };
        }

        /*
        public static GraphSources[] CreateGraphSourcesFromGraphModel(GraphModel model) 
        {
            GraphSources[] sources = new GraphSources[model.DataSources.Length];

            for (int i = 0; i < model.DataSources.Length; i++)
            {
                sources[i] = new GraphSources {
                    GraphId = model.GraphId.ToString(),
                    SeriesType = model.DataSources[i].SeriesType.ToString(),
                    DatasetName = model.DataSources[i].DatasetName,
                    ColumnNames = model.DataSources[i].ColumnNames
                };
            }

            foreach (GraphSourceModel m in model.DataSources) {
                sources.
            }

            GraphSources[] sources = new GraphSources[numSeries];

            foreach (int i in numSeries) {
                
            }

            
            */
        
    }
}