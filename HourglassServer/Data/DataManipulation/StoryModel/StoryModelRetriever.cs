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
            Story story = db.Story.First(story => story.StoryId == storyId);
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
            List<StoryBlockModel> graphBlocks = GetGraphStoryBlockByStoryId(db, storyId);
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
        public static List<StoryBlockModel> GetGraphStoryBlockByStoryId(HourglassContext db, string storyId)
        {
            var storyBlockGraphBlockJoin = from storyBlock in db.StoryBlock
                      join graphBlock in db.GraphBlock
                          on storyBlock.BlockId equals graphBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          graphBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          graphBlock.GraphId
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var joinedGraphBlock in storyBlockGraphBlockJoin)
            {
                GraphBlock graphBlock = StoryFactory.CreateGraphBlock(joinedGraphBlock.BlockId, joinedGraphBlock.GraphId);
                storyBlocks.Add(new StoryBlockModel(graphBlock, joinedGraphBlock.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetGeoMapBlocksByStoryId(HourglassContext db, string storyId)
        {
            var storyBlockGeoMapBlockJoin = from storyBlock in db.StoryBlock
                      join geomapBlock in db.GeoMapBlock
                          on storyBlock.BlockId equals geomapBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          geomapBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          geomapBlock.GeoMapId
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var joinedGeoMapBlock in storyBlockGeoMapBlockJoin)
            {
                GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlock(joinedGeoMapBlock.BlockId, joinedGeoMapBlock.GeoMapId);
                storyBlocks.Add(new StoryBlockModel(geoMapBlock, joinedGeoMapBlock.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetTextBlocksByStoryId(HourglassContext db, string storyId)
        {
            var storyBlockTextBlockJoin = from storyBlock in db.StoryBlock
                      join textBlock in db.TextBlock
                          on storyBlock.BlockId equals textBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          textBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          textBlock.EditorState
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var joinedTextBlock in storyBlockTextBlockJoin)
            {
                TextBlock textBlock = StoryFactory.CreateTextBlock(joinedTextBlock.BlockId, joinedTextBlock.EditorState);
                storyBlocks.Add(new StoryBlockModel(textBlock, joinedTextBlock.position));
            }
            return storyBlocks;
        }

    }
}
