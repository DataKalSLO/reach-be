using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
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
        public GraphSourceModel[] DataSources { get; set; }
        public string GraphOptions { get; set; }
    }
}
