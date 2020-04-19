using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Bookmark;
using HourglassServer.Models.Persistent;

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
        public List<Object> GetBookmarks(string UserId)
        {
            List<Object> bookmarks = new List<Object>();

            List<GraphBookmark> graphBookmarks = _context.GraphBookmark.Where(b => b.UserId == UserId).ToList();
            List<StoryBookmark> storyBookmarks = _context.StoryBookmark.Where(b => b.UserId == UserId).ToList();
            List<GeoMapBookmark> geoMapBookmarks = _context.GeoMapBookmark.Where(b => b.UserId == UserId).ToList();

            bookmarks.AddRange(graphBookmarks);
            bookmarks.AddRange(storyBookmarks);
            bookmarks.AddRange(geoMapBookmarks);

            return bookmarks;
        }

        [HttpPost()]
        public IActionResult CreateBookmark(Bookmark Bookmark)
        {
            switch(Bookmark.Type)
            {
                case ContentType.GRAPH:
                    _context.GraphBookmark.Add(new GraphBookmark()
                    {
                        UserId = Bookmark.UserId,
                        GraphId = Bookmark.ItemId
                    });
                    break;
                case ContentType.STORY:
                    _context.StoryBookmark.Add(new StoryBookmark()
                    {
                        UserId = Bookmark.UserId,
                        StoryId = Bookmark.ItemId
                    });
                    break;
                case ContentType.GEOMAP:
                    _context.GeoMapBookmark.Add(new GeoMapBookmark()
                    {
                        UserId = Bookmark.UserId,
                        GeoMapId = Bookmark.ItemId
                    });
                    break;
                default:
                    return BadRequest("Could not identify content type: " + Bookmark.Type);
            }

            try
            {
                _context.SaveChanges();
            } catch(Exception e)
            {
                return new BadRequestObjectResult(e);
            }
           
            return new OkObjectResult("success");
        }
    }
}
