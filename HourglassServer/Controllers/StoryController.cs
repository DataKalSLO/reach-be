﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;
using Newtonsoft.Json.Linq;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private HourglassContext _context;

        public StoryController(HourglassContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IList<StoryApplicationModel> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public StoryApplicationModel Get(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CreateStory([FromBody] StoryApplicationModel story)
        {
            try
            {
                StoryApplicationModel storyCreated = StoryModelCreator.AddStoryApplicationModelToDatabaseContext(_context, story);
                _context.SaveChanges();
                return new OkObjectResult(storyCreated);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPut]
        public string Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
