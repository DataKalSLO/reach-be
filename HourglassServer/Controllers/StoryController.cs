using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.StoryModel;
using HourglassServer.Data;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private CentralToastContext _context;

        public StoryController(CentralToastContext context)
        {
            _context = context;
        }

        // GET: api/Story
        [HttpGet]
        public IList<Story> Get()
        {
            return _context.Story.ToList(); 
        }

        // GET: api/Story/UUID
        [HttpGet("{id}")]
        public Story Get(string id)
        {
            return _context.Story.Where(a => a.storyid == id).Single();
        }

        // POST api/<controller>
        [HttpPost]
        public Story Post([FromBody] StoryCreationObject story)
        {
            Story newStory = new Story();
            newStory.setStateFromCreationObject(story);
            _context.Story.Add(newStory);
            _context.SaveChanges(); 
            return newStory;
        }

        // PUT api/<controller>/5
        [HttpPut]
        public string Put()
        {
            return "Updating stories not yet implemented";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public Story Delete(string id)
        {
            Story storyToDelete = _context.Story.First(p => p.storyid == id);
            _context.Remove(storyToDelete);
            _context.SaveChanges();
            return storyToDelete;
        }

        [HttpGet("count")]
        public string getNumberOfStories()
        {
            //Example use of Context: Access stories
            return _context.Story.Count().ToString();
        }
    }
}
