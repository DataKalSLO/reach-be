using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.Bookmark;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Data.Application.StoryModel;

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

            var graphIds = _context.GraphBookmark.Where(b => b.UserId == UserId).Select(r => r.GraphId);
            var graphs = _context.Graph.Where(r => graphIds.Contains(r.GraphId));

            var storyIds = _context.StoryBookmark.Where(b => b.UserId == UserId).Select(r => r.StoryId).ToList();
            var stories = StoryModelRetriever.GetListOfStoryModelsByID(_context, storyIds);

            var geoMapIds = _context.GeoMapBookmark.Where(b => b.UserId == UserId).Select(r => r.GeoMapId);
            var maps = _context.GeoMap.Where(r => geoMapIds.Contains(r.GeoMapId));

            foreach (StoryApplicationModel story in stories)
            {
                bookmarks.Add(new BookmarkContent()
                {
                    Type = ContentType.STORY,
                    Content = story
                });
            }

            foreach (GeoMap geoMap in maps)
            {
                bookmarks.Add(new BookmarkContent()
                {
                    Type = ContentType.GEOMAP,
                    Content = geoMap
                });
            }

            foreach (Graph graph in graphs)
            {
                bookmarks.Add(new BookmarkContent()
                {
                    Type = ContentType.GRAPH,
                    Content = graph
                });
            }

            return bookmarks;
        }

        [HttpPost]
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
                return new OkObjectResult("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
