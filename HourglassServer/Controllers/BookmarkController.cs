// <copyright file="BookmarkController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HourglassServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HourglassServer.Custom.Constraints;
    using HourglassServer.Custom.Exception;
    using HourglassServer.Data;
    using HourglassServer.Data.DataManipulation.BookmarkOperations;
    using HourglassServer.Data.DataManipulation.DbSetOperations;
    using HourglassServer.Models.Persistent;
    using Microsoft.AspNetCore.Mvc;


    [DefaultControllerRoute]
    public class BookmarkController : Controller
    {
        private readonly HourglassContext context;

        public BookmarkController(HourglassContext context)
        {
            this.context = context;
        }

        [HttpGet("geomap")]
        public IActionResult GetGeoMapBookmarks()
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                constraintChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);
                string userId = HttpContext.User.GetUserId();
                List<string> geoMapIdsBookmarked = BookmarkRetriever.GetBookmarkGeoMapByUserId(context, userId);
                return new OkObjectResult(geoMapIdsBookmarked);
            });
        }

        [HttpGet("graph")]
        public IActionResult GetGraphBookmarks()
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                string userId = HttpContext.User.GetUserId();
                List<string> graphIdsBookmarked = BookmarkRetriever.GetBookmarkGraphByUserId(context, userId);
                return new OkObjectResult(graphIdsBookmarked);
            }); 
        }

        [HttpGet("story")]
        public IActionResult GetStoryBookmarks()
        {
            return ExceptionHandler.TryApiAction(this, () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                string userId = HttpContext.User.GetUserId();
                List<string> storiesIdsBookmarked = BookmarkRetriever.GetBookmarkStoryByUserId(context, userId);
                return new OkObjectResult(storiesIdsBookmarked);
            });
        }

        [HttpPost("graph")]
        public async Task<IActionResult> ToggleGraphBookmark([FromBody] BookmarkGraph graphBookmark)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                graphBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkGraph, graphBookmark);
                await context.SaveChangesAsync();
                return new OkObjectResult(state);
            });
        }

        [HttpPost("geomap")]
        public async Task<IActionResult> ToggleGeoMapBookmark([FromBody] BookmarkGeoMap geoMapBookmark)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                geoMapBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkGeoMap, geoMapBookmark);
                await context.SaveChangesAsync();

                return new OkObjectResult(state);
            });
        }


        [HttpPost("story")]
        public async Task<IActionResult> ToggleStoryBookmark([FromBody] BookmarkStory storyBookmark)
        {
            return await ExceptionHandler.TryAsyncApiAction(this, async () =>
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                storyBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkStory, storyBookmark);
                await context.SaveChangesAsync();
                return new OkObjectResult(state);
            });
        }
    }
}
