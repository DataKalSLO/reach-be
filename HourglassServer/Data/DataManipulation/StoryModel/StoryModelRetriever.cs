using System;
using System.Linq;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;

/* Responsibility: Retrieve Stories through some query method.
 * 
 * Current Queries Implemented: By ID
 * Pending: Containing substring in description/title, by author, by date
 */
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelRetriever
    {
        public static StoryApplicationModel GetStoryApplicationModelById(HourglassContext db, string storyId)
        {
            Story story = db.Story.First(story => story.StoryId == storyId); //TODO: Replace with `Find`
            return GetStoryApplicationModelFromStory(db, story);
        }

        public static IList<StoryApplicationModel> GetAllStoryApplicationModels(HourglassContext db)
        {
            List<Story> stories = db.Story.ToList();
            List<StoryApplicationModel> storyModels = new List<StoryApplicationModel>();
            foreach (var story in stories)
                storyModels.Add(GetStoryApplicationModelFromStory(db, story));
            return storyModels;
        }

        public static StoryApplicationModel GetStoryApplicationModelFromStory(HourglassContext db, Story story)
        {
            string storyId = story.StoryId;
            List<StoryBlockModel> storyBlocks = GetStoryBlocksByStoryId(db, storyId);
            StoryApplicationModel model = new StoryApplicationModel(story);
            model.StoryBlocks = storyBlocks;
            return model;
        }

        public static List<StoryBlockModel> GetStoryBlocksByStoryId(HourglassContext db, string storyId)
        {
            List<StoryBlockModel> graphBlocks = GetGraphBlockByStoryId(db, storyId);
            List<StoryBlockModel> mapBlocks = GetGeoMapBlocksByStoryId(db, storyId);
            List<StoryBlockModel> textBlocks = GetTextBlocksByStoryId(db, storyId);
            List<StoryBlockModel> allStories = graphBlocks.Concat(textBlocks)
                                    .Concat(mapBlocks)
                                    .ToList();
            allStories.Sort();
            return allStories;
        }

        /* The reason these blocks cannot be generalized is because no supertype can be
         * declared for StoryBlocks. The script we use to generate the classes specifically
         * mentions this as one of its limitations.
         */
        public static List<StoryBlockModel> GetGraphBlockByStoryId(HourglassContext db, string storyId)
        {
            List<GraphBlock> storyBlockGraphBlockJoin = db.GraphBlock.Where(graphBlock => graphBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (GraphBlock graphBlock in storyBlockGraphBlockJoin)
                storyBlocks.Add(new StoryBlockModel(graphBlock));
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetGeoMapBlocksByStoryId(HourglassContext db, string storyId)
        {
            List<GeoMapBlock> geoMapBlocks = db.GeoMapBlock.Where(geoMapBlock => geoMapBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (GeoMapBlock geoMapBlock in geoMapBlocks)
                storyBlocks.Add(new StoryBlockModel(geoMapBlock));
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetTextBlocksByStoryId(HourglassContext db, string storyId)
        {
            List<TextBlock> textBlocks = db.TextBlock.Where(textBlock => textBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (TextBlock textBlock in textBlocks)
                storyBlocks.Add(new StoryBlockModel(textBlock));
            return storyBlocks;
        }
    }
}
