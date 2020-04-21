using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HourglassServer.Data;
using HourglassServer.Data.BookmarkModel;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Controllers;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private readonly HourglassContext _context;

        public BookmarkController(HourglassContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateBookmark(Bookmark Bookmark)
        {
            switch (Bookmark.Type)
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

        [HttpGet("{UserId}")]
        public List<BookmarkContent> GetBookmarks(string UserId)
        {
            StoryController storyController = new StoryController(_context);
            GraphController graphController = new GraphController();
            MapController mapController = new MapController();
            List<BookmarkContent> bookmarks = new List<BookmarkContent>();

            var graphIds = _context.GraphBookmark.Where(b => b.UserId == UserId).Select(r => r.GraphId);
            var graphs = _context.Graph.Where(r => graphIds.Contains(r.GraphId)); //TODO: When function exist redirect to GraphController

            var storyIds = _context.StoryBookmark.Where(b => b.UserId == UserId).Select(r => r.StoryId).ToList();
            var stories = StoryModelRetriever.GetListOfStoryModelsByID(_context, storyIds);

            var geoMapIds = _context.GeoMapBookmark.Where(b => b.UserId == UserId).Select(r => r.GeoMapId);
            var maps = _context.GeoMap.Where(r => geoMapIds.Contains(r.GeoMapId)); //TODO: When function exist redirect to GeoMapController

            AddItemsAsBookmarkContentToBookmarks<GeoMap>(maps.ToList(), ContentType.GRAPH, bookmarks);
            AddItemsAsBookmarkContentToBookmarks<StoryApplicationModel>(stories.ToList(), ContentType.GRAPH, bookmarks);
            AddItemsAsBookmarkContentToBookmarks<Graph> (graphs.ToList(), ContentType.GRAPH, bookmarks);

            return bookmarks;
        }

        private void AddItemsAsBookmarkContentToBookmarks<T>(List<T> items, ContentType type, List<BookmarkContent> bookmarks) where T : class
        {
            foreach (T item in items)
            {
                bookmarks.Add(new BookmarkContent()
                {
                    Type = type,
                    Content = item
                });
            }
        }
    }
}
