﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.StoryModel;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class StoryController : Controller
    {
        private readonly HourglassContext _context;

        public StoryController(HourglassContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStories()
        {
            try
            {
                IList<StoryApplicationModel> storyWithId = StoryModelRetriever.GetAllStoryApplicationModels(_context);
                _context.SaveChanges();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpGet("{StoryId}")]
        public IActionResult GetStoryById(string storyId)
        {
            try
            {
                StoryApplicationModel storyWithId = StoryModelRetriever.GetStoryApplicationModelById(_context, storyId);
                _context.SaveChanges();
                return new OkObjectResult(storyWithId);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), "badValue") });
            }
        }

        [HttpPost]
        public string Post([FromBody] StoryApplicationModel story)
        {
            throw new NotImplementedException();
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
