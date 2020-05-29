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
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                constraintChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);
                string userId = HttpContext.User.GetUserId();
                List<string> geoMapIdsBookmarked = BookmarkRetriever.GetBookmarkGeoMapByUserId(context, userId);
                return new OkObjectResult(geoMapIdsBookmarked);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }

        [HttpGet("graph")]
        public IActionResult GetGraphBookmarks()
        {
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                string userId = HttpContext.User.GetUserId();
                List<string> graphIdsBookmarked = BookmarkRetriever.GetBookmarkGraphByUserId(context, userId);
                return new OkObjectResult(graphIdsBookmarked);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }

        [HttpGet("story")]
        public IActionResult GetStoryBookmarks()
        {
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                string userId = HttpContext.User.GetUserId();
                List<string> storiesIdsBookmarked = BookmarkRetriever.GetBookmarkStoryByUserId(context, userId);
                return new OkObjectResult(storiesIdsBookmarked);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }

        [HttpPost("graph")]
        public async Task<IActionResult> ToggleGraphBookmark([FromBody] BookmarkGraph graphBookmark)
        {
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                graphBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkGraph, graphBookmark);
                await context.SaveChangesAsync();
                return new OkObjectResult(state);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }

        [HttpPost("geomap")]
        public async Task<IActionResult> ToggleGeoMapBookmark([FromBody] BookmarkGeoMap geoMapBookmark)
        {
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                geoMapBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkGeoMap, geoMapBookmark);
                await context.SaveChangesAsync();

                return new OkObjectResult(state);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }


        [HttpPost("story")]
        public async Task<IActionResult> ToggleStoryBookmark([FromBody] BookmarkStory storyBookmark)
        {
            try
            {
                ConstraintChecker<BookmarkGeoMap> constraintChecker = new ConstraintChecker<BookmarkGeoMap>(
                                    new ConstraintEnvironment(this.HttpContext.User, context), null);
                storyBookmark.UserId = HttpContext.User.GetUserId();
                ToggleState state = Toggler.ToggleEntity(this.context.BookmarkStory, storyBookmark);
                await context.SaveChangesAsync();
                return new OkObjectResult(state);
            }
            catch (Exception e)
            {
                return this.BadRequest(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }
    }
}
