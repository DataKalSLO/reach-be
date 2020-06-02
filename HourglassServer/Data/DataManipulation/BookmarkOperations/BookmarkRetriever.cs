using System.Collections.Generic;
using System.Linq;

namespace HourglassServer.Data.DataManipulation.BookmarkOperations
{
    public class BookmarkRetriever
    {
        public static List<string> GetBookmarkGeoMapByUserId(HourglassContext context, string userId)
        {
            return context.BookmarkGeoMap
                    .Where(geoMapBookmark => geoMapBookmark.UserId == userId)
                    .Select(bs => bs.GeoMapId)
                    .ToList();
        }

        public static List<string> GetBookmarkGraphByUserId(HourglassContext context, string userId)
        {
            return context.BookmarkGraph
                    .Where(graphBookmark => graphBookmark.UserId == userId)
                    .Select(bs => bs.GraphId)
                    .ToList();
        }

        public static List<string> GetBookmarkStoryByUserId(HourglassContext context, string userId)
        {
            return context.BookmarkStory
                    .Where(storyBookmark => storyBookmark.UserId == userId)
                    .Select(bs => bs.StoryId)
                    .ToList();
        }
    }
}
