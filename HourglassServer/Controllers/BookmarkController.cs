using System;
using System.Collections.Generic;
using System.Linq;
using HourglassServer.Data;
using HourglassServer.Data.Application.BookmarkModel;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;


namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private readonly HourglassContext _context;
        private const string errorType = "badValue";

        public BookmarkController(HourglassContext context)
        {
            _context = context;
        }

        // TODO: Remove paramters for UserId and use token instead

        [HttpGet("GeoMap/{userId}")]
        public IActionResult GetGeoMapBookmarks(string userId)
        {
            try
            {
                List<string> geoMapIdsBookmarked = _context.BookmarkGeoMap.Where(geoMapBookmark => geoMapBookmark.UserId == userId).Select(bs => bs.GeoMapId).ToList();
                List<GeoMap> geoMapBookmarked = _context.GeoMap.Where(geoMap => geoMapIdsBookmarked.Contains(geoMap.GeoMapId)).ToList(); //TODO: Route to application model when logic exists.
                return new OkObjectResult(geoMapBookmarked);
            }catch(Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }
        
        [HttpGet("Graph/{userId}")]
        public IActionResult GetGraphBookmarks(string userId)
        {
            try
            {
                List<string> graphIdsBookmarked = _context.BookmarkGraph.Where(graphBookmark => graphBookmark.UserId == userId).Select(bs => bs.GraphId).ToList();
                List<Graph> graphsBookmarked = _context.Graph.Where(graph => graphIdsBookmarked.Contains(graph.GraphId)).ToList();  //TODO: Route to application model when logic exists.
                return new OkObjectResult(graphsBookmarked);
            }catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        [HttpGet("Story/{userId}")]
        public IActionResult GetStoryBookmarks(string userId)
        {
            try
            {
                List<string> storiesIdsBookmarked = _context.BookmarkStory.Where(storyBookmark => storyBookmark.UserId == userId).Select(bs => bs.StoryId).ToList();
                List<StoryApplicationModel> storiesBookmarked = new List<StoryApplicationModel>();
                foreach (string storyId in storiesIdsBookmarked)
                    storiesBookmarked.Add(StoryModelRetriever.GetStoryApplicationModelById(_context, storyId));
                return new OkObjectResult(storiesBookmarked);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        // TODO: Consider merging creating/deleting. Delete if bookmark already exists and add if not.
        // Reasoning: FEND can then just map a button to a single http route.

        [HttpPost]
        public IActionResult CreateBookmark([FromBody] BookmarkApplicationModel bookmark)
        {
            try
            {
                MutateBookmark(MutatorOperations.ADD, bookmark);
                _context.SaveChanges();
                return new OkResult();
            } catch(Exception e){
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        [HttpDelete]
        public IActionResult DeleteBookmark([FromBody] BookmarkApplicationModel bookmark)
        {
            try
            {
                MutateBookmark(MutatorOperations.DELETE, bookmark);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        private void MutateBookmark (MutatorOperations operation, BookmarkApplicationModel bookmark)
        {
            switch (bookmark.ContentType)
            {
                case ContentType.GEOMAP:
                    DbSetMutator.PerformOperationOnDbSet<BookmarkGeoMap>(_context.BookmarkGeoMap, operation, new BookmarkGeoMap { GeoMapId = bookmark.ContentId, UserId = bookmark.UserId });
                    break;
                case ContentType.GRAPH:
                    DbSetMutator.PerformOperationOnDbSet<BookmarkGraph>(_context.BookmarkGraph, operation, new BookmarkGraph { GraphId = bookmark.ContentId, UserId = bookmark.UserId });
                    break;
                case ContentType.STORY:
                    DbSetMutator.PerformOperationOnDbSet<BookmarkStory>(_context.BookmarkStory, operation, new BookmarkStory { StoryId = bookmark.ContentId, UserId = bookmark.UserId });
                    break;
                default:
                    throw new InvalidOperationException(String.Format("Could not identify content type: ", bookmark.ContentType));
            }
        }
    }
}
