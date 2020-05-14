using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using HourglassServer.Data;
using System.Linq;

namespace HourglassServer.Controllers { 
    [DefaultControllerRoute]
    [EnableCors("SiteCorsPolicy")]
    public class SearchController : Controller {
        private static String elasticURL = "https://search-hourglass-search-test-boatibipr2tvrekti6tuz7pghi.us-east-2.es.amazonaws.com/_search?q=";

        public SearchController() {}

        /* Return elasticsearch result using sent query */

        // POST _search/<query>
        [HttpPost]
        async public Task<String> Search([FromBody] String query) {
            String response = "Default Return String";
            using (var client = new HttpClient()) {
                try {
                    response = await client.GetStringAsync(elasticURL + query);
                } catch (Exception e) {
                    return (e.Message.ToString());
                }
            }
            return response;
        }

        /* Return list of JSON objects of all graphs, each object has  title and graph ID */
        /* Method not yet implemented */
        // GET _search/graphs
        [HttpPost]
        public List<String> GetGraphsSearch() {
            List<String> JSONGraphs = new List<String>();
            // TODO: Get List<Graph> object from Context
            return JSONGraphs;
        }


        /* Return list of JSON object of all stories, each object has story title and story ID */
        /* Method not yet implemented */
        // GET _search/stories
        [HttpGet]
        public List<String> GetStoriesSearch() {
            List<String> JSONStories = new List<String>();
            // TODO: Get List<Story> object from Context
            return JSONStories;
        }
    }
}