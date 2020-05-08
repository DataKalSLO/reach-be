using Newtonsoft.Json.Linq;
using System;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphModel
    {
        public Guid GraphId { get; set; }
        public string UserId { get; set; }
        public long Timestamp { get; set; }
        public string GraphTitle { get; set; }
        public string SnapshotUrl { get; set; }
        public GraphSourceModel[] DataSources { get; set; }
        public JObject GraphOptions { get; set; }
    }
}
