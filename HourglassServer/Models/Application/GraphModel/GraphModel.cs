using Newtonsoft.Json.Linq;

namespace HourglassServer.Data.Application.GraphModel
{
    public class GraphModel
    {
        public string GraphId { get; set; }
        public string UserId { get; set; }
        public string GraphTitle { get; set; }
        public string GraphSVG { get; set; }
        public string GraphCategory { get; set; }
        public GraphSourceModel[] DataSources { get; set; }
        public JObject GraphOptions { get; set; }
    }
}
