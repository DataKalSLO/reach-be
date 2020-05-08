using Newtonsoft.Json.Linq;
using System;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class GraphModel
    {
        public string GraphId { get; set; }
        public string UserId { get; set; }
        public string GraphTitle { get; set; }
        public string GraphSVG { get; set; }
        public GraphSourceModel[] DataSources { get; set; }
        public JObject GraphOptions { get; set; }
    }
}
