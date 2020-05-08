using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphSourceModel
    {
        public string DatasetName { get; set; }
        public string[] ColumnNames { get; set; }
        public SeriesType SeriesType { get; set; }

        public static GraphSourceModel[] convertGraphSources(GraphSource[] persistentModel) 
        {
            GraphSourceModel[] graphSources = new GraphSourceModel[persistentModel.Length];

            for (int i = 0; i < persistentModel.Length; i++) {
                graphSources[i] = convertGraphSource(persistentModel[i]);
            }

            return graphSources;
        }

        public static GraphSourceModel convertGraphSource(GraphSource persistentModel) 
        {
            SeriesType seriesType;

            if (!Enum.TryParse(persistentModel.SeriesType, out seriesType)) 
            {
                throw new Exception (String.Format("Bad series type in graph source: {0}.", persistentModel.SeriesType));
            }

            return new GraphSourceModel 
            {
                DatasetName = persistentModel.DatasetName,
                ColumnNames = persistentModel.ColumnNames,
                SeriesType = seriesType
            };
        }
    }
}
