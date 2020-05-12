using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphSourceModel
    {
        public string DatasetName { get; set; }
        public string[] ColumnNames { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] // Strings not converted by default
        public SeriesType SeriesType { get; set; }

        public static GraphSourceModel[] convertPersistentGraphSource(GraphSource[] persistentModel)
        {
            GraphSourceModel[] graphSources = new GraphSourceModel[persistentModel.Length];

            for (int i = 0; i < persistentModel.Length; i++)
            {
                Enum.TryParse(persistentModel[i].SeriesType, out SeriesType seriesType);
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
