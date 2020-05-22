using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using HourglassServer.Data.DataManipulation.GraphOperations;

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

        public async Task<GraphApplicationModel> CreateGraph(HourglassContext db, string currentUserId) {
            return await GraphModelCreator.CreateGraph(db, this, currentUserId);
        }
        public async Task<GraphApplicationModel> UpdateGraph(HourglassContext db, string currentUserId) {
            return await GraphModelUpdater.UpdateGraph(db, this, currentUserId);
        }
    }
}
