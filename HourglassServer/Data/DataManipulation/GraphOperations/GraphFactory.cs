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

            return new Graph()
            {
                GraphId = model.GraphId,
                GraphTitle = model.GraphTitle,
                UserId = model.UserId,
                Timestamp = timestamp,
                SnapshotUrl = null,
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
                graphSources[i] = new GraphSource
                {
                    GraphId = graphId,
                    SeriesType = sourceModels[i].SeriesType.ToString(),
                    DatasetName = sourceModels[i].DatasetName,
                    ColumnNames = sourceModels[i].ColumnNames
                };
            }

            return graphSources;
        }

        public static GraphSourceModel[] CreateGraphSourceModelFromGraphSources(GraphSource[] persistentModel)
        {
            GraphSourceModel[] graphSources = new GraphSourceModel[persistentModel.Length];

            for (int i = 0; i < persistentModel.Length; i++)
            {
                if (!Enum.TryParse(persistentModel[i].SeriesType, out SeriesType seriesType))
                {
                    throw new Exception(String.Format("Unknown series type {0}.", persistentModel[i].SeriesType));
                }
                graphSources[i] = new GraphSourceModel
                {
                    DatasetName = persistentModel[i].DatasetName,
                    ColumnNames = persistentModel[i].ColumnNames,
                    SeriesType = seriesType
                };
            }

            return graphSources;
        }

        public static DefaultGraph CreateDefaultGraph(string graphId, string category)
        {
            return new DefaultGraph()
            {
                GraphId = graphId,
                Category = category
            };
        }

        public static GraphApplicationModel CreateGraphApplicationModel(Graph graph, GraphSource[] sources)
        {
            return new GraphApplicationModel
            {
                GraphId = graph.GraphId,
                UserId = graph.UserId,
                UserName = graph.User.Name,
                TimeStamp = graph.Timestamp.Value,
                GraphTitle = graph.GraphTitle,
                SnapshotUrl = graph.SnapshotUrl,
                DataSources = CreateGraphSourceModelFromGraphSources(sources),
                GraphOptions = graph.GraphOptions
            };
        }
    }
}