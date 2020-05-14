// <copyright file="BookmarkController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using HourglassServer.Data;
    using HourglassServer.Data.DataManipulation.DbSetOperations;
    using HourglassServer.Models.Persistent;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private const string ErrorType = "badValue";
        private const string NoOwnershipError = "The authenticated user is not the owner of this UserId: {0}";
        private const string MissingTokenError = "This operations requires a user token. None found.";
        private readonly HourglassContext context;

        public BookmarkController(HourglassContext context)
        {
            this.context = context;
        }

        [HttpGet("geomap")]
        public IActionResult GetGeoMapBookmarks()
        {
            try
            {
                string userId = Utilities.GetUserIdFromToken(this.HttpContext.User.Claims);
                List<string> geoMapIdsBookmarked = this.context.BookmarkGeoMap
                    .Where(geoMapBookmark => geoMapBookmark.UserId == userId)
                    .Select(bs => bs.GeoMapId).ToList();
                return new OkObjectResult(geoMapIdsBookmarked);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }

        [HttpGet("graph")]
        public IActionResult GetGraphBookmarks()
        {
            try
            {
                string userId = Utilities.GetUserIdFromToken(this.HttpContext.User.Claims);
                List<string> graphIdsBookmarked = this.context.BookmarkGraph
                    .Where(graphBookmark => graphBookmark.UserId == userId)
                    .Select(bs => bs.GraphId)
                    .ToList();
                return new OkObjectResult(graphIdsBookmarked);
            }catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }

        [HttpGet("story")]
        public IActionResult GetStoryBookmarks()
        {
            try
            {
                string userId = Utilities.GetUserIdFromToken(this.HttpContext.User.Claims);
                List<string> storiesIdsBookmarked = this.context.BookmarkStory
                    .Where(storyBookmark => storyBookmark.UserId == userId)
                    .Select(bs => bs.StoryId)
                    .ToList();
                return new OkObjectResult(storiesIdsBookmarked);
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }

        [HttpPost("graph")]
        public IActionResult ToggleGraphBookmark([FromBody] BookmarkGraph graphBookmark)
        {
            try
            {
                this.AssertAuthenticationTokenUserIdMatchesString(graphBookmark.UserId);
                return new OkObjectResult(this.ToggleBookmark(this.context.BookmarkGraph, graphBookmark));
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }

        [HttpPost("geomap")]
        public IActionResult ToggleGeoMapBookmark([FromBody] BookmarkGeoMap requestedGeoMapBookmark)
        {
            try
            {
                this.AssertAuthenticationTokenUserIdMatchesString(requestedGeoMapBookmark.UserId);
                return new OkObjectResult(this.ToggleBookmark(this.context.BookmarkGeoMap, requestedGeoMapBookmark));
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }


        [HttpPost("story")]
        public IActionResult ToggleStoryBookmark([FromBody] BookmarkStory requestStoryBookmark)
        {
            try
            {
                this.AssertAuthenticationTokenUserIdMatchesString(requestStoryBookmark.UserId);
                return new OkObjectResult(this.ToggleBookmark(this.context.BookmarkStory, requestStoryBookmark));
            }
            catch (Exception e)
            {
                return this.BadRequest(new[] { new HourglassError(e.ToString(), ErrorType) });
            }
        }

        /*
         * Private Generalized Methods. No public facing endpoints.
         */

        private void AssertAuthenticationTokenUserIdMatchesString(string userId)
        {
            string tokenUserId = Utilities.GetUserIdFromToken(this.HttpContext.User.Claims);
            if (tokenUserId != userId)
            {
                throw new InvalidOperationException(string.Format(NoOwnershipError, userId));
            }
        }

        private BookmarkState ToggleBookmark<T>(DbSet<T> dbSet, T requestedBookmark)
            where T : class
        {
            bool bookmarkIsEnabled;
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

            this.context.SaveChanges();
            return new BookmarkState(bookmarkIsEnabled);
        }
    }

    internal class BookmarkState
    {
        public BookmarkState(bool enabled)
        {
            this.Enabled = enabled;
        }

        private bool Enabled { get; set; }
    }
}
