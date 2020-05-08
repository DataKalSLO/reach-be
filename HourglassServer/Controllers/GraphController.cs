using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.GraphOperations;
using HourglassServer.Models.Persistent;
using Newtonsoft.Json;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class GraphController : Controller
    {
        private readonly HourglassContext _context;

        public GraphController(HourglassContext context)
        {
            _context = context;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get()
        {
            return "Retrieving graphs not yet implemented";
        }

        //[UserExists]
        [HttpPost]
        public IActionResult UpdateGraph([FromBody] NewGraphRequestModel requestedGraphUpdate)
        {
            try 
            {
                // If no graph id is provided, create a new graph object
                bool isNewGraph = requestedGraphUpdate.GraphId == null;

                GraphModel result = new GraphModel();

                // Create a new guid for the graph, or update using the provided id
                result.GraphId = isNewGraph ? Guid.NewGuid() : requestedGraphUpdate.GraphId;
                result.Timestamp = requestedGraphUpdate.Timestamp;
                result.GraphTitle = requestedGraphUpdate.GraphTitle;

                // Upload the image to S3


                //var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;
                var guid = Guid.NewGuid();

                GraphSnapshotOperations.UploadSnapshotToS3(requestedGraphUpdate.GraphSVG);
                
                //var xDataSource = JsonConvert.SerializeObject(graph.XDataSource);
                //var back = JsonConvert.DeserializeObject<DataSourceModel>(xDataSource);

                //GraphApplicationModel result;


                //Console.WriteLine(userId);

                return new OkResult();

                //if (HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value == person.Email)
                    //Console.WriteLine( graph.GraphOptions.ToString(Formatting.None));
            }/*
            try
            {
                StoryApplicationModel storyCreated = StoryModelCreator.AddStoryApplicationModelToDatabaseContext(_context, story);
                _context.SaveChanges();
                return new OkObjectResult(storyCreated.Id);
            }*/
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating Graphs is not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete()
        {
            return "Deleting Graphs is not yet implemented";
        }
    }
}
