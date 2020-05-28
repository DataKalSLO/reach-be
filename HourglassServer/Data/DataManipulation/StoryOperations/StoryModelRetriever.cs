/* Responsibility: Retrieve Stories through some query method.
 * 
 * Current Queries Implemented: By ID
 * Pending: Containing substring in description/title, by author, by date
 */
namespace HourglassServer.Data.DataManipulation.StoryOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Models.Persistent;
    using HourglassServer.Data.DataManipulation.StoryOperations;

    // TODO: Replace `ToList` to `ToListAsync` and convert to async queries
    public static class StoryModelRetriever
    {
        /*
         * Retrieving stories in a certain status
         */
        public static IList<StoryApplicationModel> GetStoryApplicationModelsInPublicationStatusByUserId(HourglassContext db, PublicationStatus expectedStatus, string userId)
        {
            IList<Story> storiesInDraftStatus = GetStoriesInPublicationStatus(db, expectedStatus);
            IList<Story> userDraftStories = storiesInDraftStatus.Where(story => story.UserId == userId).ToList();
            return GetStoryApplicationListFromStories(db, userDraftStories);
        }

        public static IList<StoryApplicationModel> GetStoryApplicationModelsInPublicationStatus(HourglassContext db, PublicationStatus expectedStatus)
        {
            IList<Story> storiesInStatus = GetStoriesInPublicationStatus(db, expectedStatus);
            return GetStoryApplicationListFromStories(db, storiesInStatus);
        }

        public static StoryApplicationModel GetStoryApplicationModelById(HourglassContext db, string storyId)
        {
            Story story = db.Story.First(story => story.StoryId == storyId); // TODO: Replace with `Find`
            return GetStoryApplicationModelFromStory(db, story);
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

        /*
         * (private) Helper Methods
         */

        private static IList<Story> GetStoriesInPublicationStatus(HourglassContext db, PublicationStatus expectedStatus)
        {
            return db.Story
                .ToList() // translate to enumerable before custom query below
                .Where(story => StoryFactory.StoryIsInStatus(story, expectedStatus))
                .ToList();
        }

        private static IList<StoryApplicationModel> GetStoryApplicationListFromStories(HourglassContext db, IList<Story> stories)
        {
            List<StoryApplicationModel> storyModels = new List<StoryApplicationModel>();
            foreach (var story in stories)
            {
                storyModels.Add(GetStoryApplicationModelFromStory(db, story));
            }
            return storyModels;
        }


        /* The reason these blocks cannot be generalized is because no supertype can be
         * declared for StoryBlocks. The script we use to generate the classes specifically
         * mentions this as one of its limitations.
         */
        private static List<StoryBlockModel> GetGraphBlockByStoryId(HourglassContext db, string storyId)
        {
            List<GraphBlock> storyBlockGraphBlockJoin = db.GraphBlock.Where(graphBlock => graphBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (GraphBlock graphBlock in storyBlockGraphBlockJoin)
            {
                storyBlocks.Add(new StoryBlockModel(graphBlock));
            }

            return storyBlocks;
        }

        private static List<StoryBlockModel> GetGeoMapBlocksByStoryId(HourglassContext db, string storyId)
        {
            List<GeoMapBlock> geoMapBlocks = db.GeoMapBlock.Where(geoMapBlock => geoMapBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (GeoMapBlock geoMapBlock in geoMapBlocks)
            {
                storyBlocks.Add(new StoryBlockModel(geoMapBlock));
            }

            return storyBlocks;
        }

        private static List<StoryBlockModel> GetTextBlocksByStoryId(HourglassContext db, string storyId)
        {
            List<TextBlock> textBlocks = db.TextBlock.Where(textBlock => textBlock.StoryId == storyId).ToList();
            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (TextBlock textBlock in textBlocks)
            {
                storyBlocks.Add(new StoryBlockModel(textBlock));
            }

            return storyBlocks;
        }
    }
}
