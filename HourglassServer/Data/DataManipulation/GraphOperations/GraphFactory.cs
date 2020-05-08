using System;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.GraphModel;
using Newtonsoft.Json;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphFactory
    {
        public static string GenerateNewGraphId() 
        {
            return Guid.NewGuid().ToString();
        }

        public static Graph CreateGraphFromGraphModel(GraphModel model) 
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var graphOptions = model.GraphOptions.ToString(Formatting.None);
            var snapshotUrl = GraphSnapshotOperations.UploadSnapshotToS3(model.GraphSVG);

            return new Graph() 
            {
                GraphId = model.GraphId,
                GraphTitle = model.GraphTitle,
                UserId = model.UserId,
                Timestamp = timestamp,
                SnapshotUrl = snapshotUrl,
                GraphOptions = graphOptions
            };
        }

        public static GraphSource[] CreateGraphSourcesFromGraphSourceModel(
            GraphSourceModel[] sourceModels, 
            string graphId) 
        {
            GraphSource[] graphSources = new GraphSource[sourceModels.Length];

            for (int i = 0; i < sourceModels.Length; i++)
            {
                graphSources[i] = new GraphSource {
                    GraphId = graphId,
                    SeriesType = sourceModels[i].SeriesType.ToString(),
                    DatasetName = sourceModels[i].DatasetName,
                    ColumnNames = sourceModels[i].ColumnNames
                };
            }

            return graphSources;
        }
    }
}