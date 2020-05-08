using Newtonsoft.Json.Linq;
using System;

namespace HourglassServer.Data.Application.GraphModel
{
    public partial class NewGraphRequestModel
    {
        public Guid GraphId { get; set; }
        public long Timestamp { get; set; }
        public string GraphTitle { get; set; }
        public GraphSourceModel XDataSource { get; set; }
        public GraphSourceModel YDataSource { get; set; }
        public GraphSourceModel StackDataSource { get; set; }
        public JObject GraphOptions { get; set; }
        public string GraphSVG { get; set; } 
    }
}
