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
    }
}
