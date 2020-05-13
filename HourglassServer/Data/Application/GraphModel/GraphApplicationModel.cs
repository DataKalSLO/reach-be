using Newtonsoft.Json;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.GraphModel
{
    public class GraphApplicationModel
    {
        public string GraphId { get; set; }
        public string UserId { get; set; }
        public long TimeStamp { get; set; }
        public string GraphTitle { get; set; }
        public string SnapshotUrl { get; set; }

        [JsonConverter(typeof(GraphSourceModel))]
        public GraphSource[] DataSources { get; set; }

        public string GraphOptions { get; set; }
    }
}
