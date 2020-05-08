using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphSourceModel
    {
        public string DatasetName { get; set; }
        public string[] ColumnNames { get; set; }
        public string SeriesType { get; set; }

        public static GraphSourceModel[] convertGraphSources(GraphSource[] persistentModel) 
        {
            GraphSourceModel[] graphSources = new GraphSourceModel[persistentModel.Length];

            for (int i = 0; i < persistentModel.Length; i++) {
                graphSources[i] = new GraphSourceModel {
                    DatasetName = persistentModel[i].DatasetName,
                    ColumnNames = persistentModel[i].ColumnNames,
                    SeriesType = persistentModel[i].SeriesType,
                };
            }

            return graphSources;
        }
    }
}
