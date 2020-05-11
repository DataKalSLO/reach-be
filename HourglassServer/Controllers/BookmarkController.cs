using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HourglassServer.Data;
using HourglassServer.Data.Application.BookmarkModel;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;
using HourglassServer.Data.DataManipulation.StoryModel;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private readonly HourglassContext _context;
        private const string errorType = "badValue";
        private const string NOT_OWNER_ERROR = "The authenticated user is not the owner of this UserId: {0}";
        private const string MISSING_TOKEN_ERROR = "This operations requires a user token. None found.";

        public BookmarkController(HourglassContext context)
        {
            _context = context;
        }

        [HttpGet("geomap")]
        public IActionResult GetGeoMapBookmarks()
        {
            try
            {
                string userId = GetUserIdFromAuthenticationToken();
                List<string> geoMapIdsBookmarked = _context.BookmarkGeoMap
                    .Where(geoMapBookmark => geoMapBookmark.UserId == userId)
                    .Select(bs => bs.GeoMapId).ToList();
                return new OkObjectResult(geoMapIdsBookmarked);
            }catch(Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }
        
        [HttpGet("graph")]
        public IActionResult GetGraphBookmarks()
        {
            try
            {
                string userId = GetUserIdFromAuthenticationToken();
                List<string> graphIdsBookmarked = _context.BookmarkGraph
                    .Where(graphBookmark => graphBookmark.UserId == userId)
                    .Select(bs => bs.GraphId)
                    .ToList();
                return new OkObjectResult(graphIdsBookmarked);
            }catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        [HttpGet("story")]
        public IActionResult GetStoryBookmarks()
        {
            try
            {
                string userId = GetUserIdFromAuthenticationToken();
                List<string> storiesIdsBookmarked = _context.BookmarkStory
                    .Where(storyBookmark => storyBookmark.UserId == userId)
                    .Select(bs => bs.StoryId)
                    .ToList();
                return new OkObjectResult(storiesIdsBookmarked);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        [HttpPost("graph")]
        public IActionResult ToggleGraphBookmark([FromBody] BookmarkGraph graphBookmark)
        {
            try
            {
                AssertAuthenticationTokenUserIdMatchesString(graphBookmark.UserId);
                return new OkObjectResult(ToggleBookmark(_context.BookmarkGraph, graphBookmark));
            } catch(Exception e){
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        [HttpPost("geomap")]
        public IActionResult ToggleGeoMapBookmark([FromBody] BookmarkGeoMap requestedGeoMapBookmark)
        {
            try
            {
                AssertAuthenticationTokenUserIdMatchesString(requestedGeoMapBookmark.UserId);
                return new OkObjectResult(ToggleBookmark(_context.BookmarkGeoMap, requestedGeoMapBookmark));
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }


        [HttpPost("story")]
        public IActionResult ToggleStoryBookmark([FromBody] BookmarkStory requestStoryBookmark)
        {
            try
            {
                AssertAuthenticationTokenUserIdMatchesString(requestStoryBookmark.UserId);
                return new OkObjectResult(ToggleBookmark(_context.BookmarkStory, requestStoryBookmark));
            }
            catch (Exception e)
            {
                return BadRequest(new[] { new HourglassError(e.ToString(), errorType) });
            }
        }

        /*
         * Private Generalized Methods. No public facing endpoints.
         */

        private string GetUserIdFromAuthenticationToken()
        {
            Claim userToken = HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .FirstOrDefault();
            if (userToken == null)
                throw new InvalidOperationException(MISSING_TOKEN_ERROR);
            return userToken.Value;
        }

        private void AssertAuthenticationTokenUserIdMatchesString(string userId)
        {
            string tokenUserId = GetUserIdFromAuthenticationToken();
            if (tokenUserId != userId)
                throw new InvalidOperationException(String.Format(NOT_OWNER_ERROR, userId));
        }

        private BookmarkState ToggleBookmark<T>(DbSet<T> dbSet, T requestedBookmark) where T : class
        {
            Boolean bookmarkIsEnabled;
            if (dbSet.Any(dbBookmark => dbBookmark == requestedBookmark))
            {
                DbSetMutator.PerformOperationOnDbSet<T>(dbSet, MutatorOperations.DELETE, requestedBookmark);
                bookmarkIsEnabled = false;
            }
            else
            {
                DbSetMutator.PerformOperationOnDbSet<T>(dbSet, MutatorOperations.ADD, requestedBookmark);
                bookmarkIsEnabled = true;
            }

            _context.SaveChanges();
            return new BookmarkState { Enabled = bookmarkIsEnabled };
        }
    }

    class BookmarkState
    {
        public bool Enabled;
    }
}
