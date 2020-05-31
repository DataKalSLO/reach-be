using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.StoryOperations;
using HourglassServer.Custom.Constraints;
using HourglassServer.Custom.Exception;
using System.Text;
using Newtonsoft.Json;

namespace HourglassServer.Controllers
{

    [DefaultControllerRoute]
    public class SearchController : Controller
    {
        private static string elasticURL = "https://search-hourglass-search-test-boatibipr2tvrekti6tuz7pghi.us-east-2.es.amazonaws.com";

        public SearchController()
        {
        }

        [HttpPost("all")]
        public async Task<IActionResult> QueryElasticsearchAll()
        {
            // Read raw body of request (query to ES) as string
            string bodyAsString;
            using (var streamReader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                bodyAsString = await streamReader.ReadToEndAsync();
            }

            // Send query to ES and give raw JSON results as response
            string esResult = "";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(elasticURL);
                using var response = await httpClient.PostAsync("/_search", new StringContent(bodyAsString, Encoding.UTF8, "application/json"));
                using var content = response.Content;
                var result = await content.ReadAsStringAsync();
                esResult = result;
            }

            return Ok(esResult);
        }
    }
}
