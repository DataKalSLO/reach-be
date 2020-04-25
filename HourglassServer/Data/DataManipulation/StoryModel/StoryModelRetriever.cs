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
            return GetStoryAppplicationModelFromStory(db, story);
            Story story = db.Story.First(a => a.StoryId == storyId);
        }

        public static IList<StoryApplicationModel> GetAllStoryApplicationModels(HourglassContext db)
        {
            List<Story> stories = db.Story.ToList();
            List<StoryApplicationModel> storyModels = new List<StoryApplicationModel>();
            foreach (var story in stories)
                storyModels.Add(GetStoryAppplicationModelFromStory(db, story));
            return storyModels;
        }

        public static StoryApplicationModel GetStoryAppplicationModelFromStory(HourglassContext db, Story story)
        {
            string storyID = story.StoryId;
            List<StoryBlockModel> storyBlocks = GetStoryBlocksByStoryID(db, storyID);
            StoryApplicationModel model = new StoryApplicationModel(story);
            model.StoryBlocks = storyBlocks;
            return model;
        }

        public static List<StoryBlockModel> GetStoryBlocksByStoryID(HourglassContext db, string storyId)
        {
            List<StoryBlockModel> graphBlocks = GetGraphStoryBlockByStoryID(db, storyId);
            List<StoryBlockModel> mapBlocks = GetGeoMapBlocksByStoryID(db, storyId);
            List<StoryBlockModel> textBlocks = GetTextBlocksByStoryID(db, storyId);
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
        public static List<StoryBlockModel> GetGraphStoryBlockByStoryID(HourglassContext db, string storyId)
        {
            var res = from storyBlock in db.StoryBlock
                      join graphBlock in db.GraphBlock
                          on storyBlock.BlockId equals graphBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          storyBlock.StoryId,
                          graphBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          graphBlock.GraphId
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var val in res)
            {
                GraphBlock graphBlock = StoryFactory.CreateGraphBlock(val.BlockId, val.GraphId);
                storyBlocks.Add(new StoryBlockModel(graphBlock, val.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetGeoMapBlocksByStoryID(HourglassContext db, string storyId)
        {
            var res = from storyBlock in db.StoryBlock
                      join geomapBlock in db.GeoMapBlock
                          on storyBlock.BlockId equals geomapBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          storyBlock.StoryId,
                          geomapBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          geomapBlock.GeoMapId
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var val in res)
            {
                GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlock(val.BlockId, val.GeoMapId);
                storyBlocks.Add(new StoryBlockModel(geoMapBlock, val.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetTextBlocksByStoryID(HourglassContext db, string storyId)
        {
            var res = from storyBlock in db.StoryBlock
                      join geomapBlock in db.TextBlock
                          on storyBlock.BlockId equals geomapBlock.BlockId
                      where storyBlock.StoryId == storyId
                      select new
                      {
                          storyBlock.StoryId,
                          geomapBlock.BlockId,
                          position = storyBlock.BlockPosition,
                          geomapBlock.EditorState
                      };

            List<StoryBlockModel> storyBlocks = new List<StoryBlockModel>();
            foreach (var val in res)
            {
                TextBlock textBlock = StoryFactory.CreateTextBlock(val.BlockId, val.EditorState);
                storyBlocks.Add(new StoryBlockModel(textBlock, val.position));
            }
            return storyBlocks;
        }

    }
}
