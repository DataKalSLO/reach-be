// Responsibility: Delete all entities associated with a Story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    using System.Collections.Generic;
    using System.Linq;
    using HourglassServer.Data.Application.StoryModel;

    public static class StoryModelDeleter
    {
        public static void DeleteStoryById(HourglassContext db, string storyId)
        {
            List<StoryBlockModel> storyBlocks = StoryModelRetriever.GetStoryBlocksByStoryId(db, storyId);
            db.GeoMapBlock.RemoveRange(db.GeoMapBlock.Where(geoMapBlock => geoMapBlock.StoryId == storyId));
            db.GraphBlock.RemoveRange(db.GraphBlock.Where(graphBlock => graphBlock.StoryId == storyId));
            db.TextBlock.RemoveRange(db.TextBlock.Where(textBlock => textBlock.StoryId == storyId));
            db.Story.Remove(db.Story.Find(storyId));
        }
    }
}
