using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.StoryModel;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private HourglassContext _context;

        public BookmarkController(HourglassContext context)
        {
            _context = context;
        }


        [HttpGet("{UserId}")]
        public StoryCreationObject GetBookmarks(string UserId)
        {
            throw new Exception("Method not implemented yet.");
        }

        [HttpPost()]
        public StoryCreationObject CreateBookmark(BookmarkController Bookmark)
        {
            throw new Exception("Method not implemented yet.");
        }
    }
}
