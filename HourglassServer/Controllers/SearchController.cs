using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using HourglassServer.Data;
using System.Linq;
using Elasticsearch.Net;

namespace HourglassServer.Controllers { 
    [DefaultControllerRoute]
    [EnableCors("SiteCorsPolicy")]
    public class SearchController : Controller {
        private static string elasticURL = "https://search-hourglass-search-test-boatibipr2tvrekti6tuz7pghi.us-east-2.es.amazonaws.com";

        public SearchController() {}

        /* Return elasticsearch result using sent query */

        // POST _search/<query>
        [HttpPost]
        async public Task<string> Search([FromBody] string qry) {
            var config =  new ConnectionConfiguration(new Uri(elasticURL));
            var lowLevelClient = new ElasticLowLevelClient(config);
            using ((IDisposable)lowLevelClient) {
                try {
                    var searchResponse = await lowLevelClient.SearchAsync<StringResponse>("search", PostData.Serializable(new 
                    {
                        query = new
                        {
                            match = new 
                            {
                                field = "title",
                                query = qry
                            }
                        }
                    }));
                    return searchResponse.Body;
                } catch (Exception e) {
                    return (e.Message.ToString());
                }
            }
        }

        /* Return list of JSON objects of all graphs, each object has  title and graph ID */
        /* Method not yet implemented */
        // GET _search/graphs
        [HttpPost]
        public List<string> GetGraphsSearch() {
            List<string> JSONGraphs = new List<string>();
            // TODO: Get List<Graph> object from Context
            return JSONGraphs;
        }


        /* Return list of JSON object of all stories, each object has story title and story ID */
        /* Method not yet implemented */
        // GET _search/stories
        [HttpGet]
        public List<string> GetStoriesSearch() {
            List<string> JSONStories = new List<string>();
            // TODO: Get List<Story> object from Context
            return JSONStories;
        }
    }
}