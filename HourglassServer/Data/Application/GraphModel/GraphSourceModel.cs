using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.GraphModel
{
    public class GraphSourceModel
    {
        public string DatasetName { get; set; }
        public string[] ColumnNames { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SeriesType SeriesType { get; set; }

        public static GraphSourceModel[] ConvertPersistentGraphSource(GraphSource[] persistentModel)
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
    }
}
