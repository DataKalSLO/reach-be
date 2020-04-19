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
        public static StoryApplicationModel GetStoryModelByID(HourglassContext db, string storyID)
        {
            Story story = db.Story.First(a => a.StoryId == storyID);
            return CreateStoryModelFromStory(db, story);
        }

        public static IList<StoryApplicationModel> GetAllStoryApplicationModels(HourglassContext db)
        {
            List<Story> stories = db.Story.ToList();
            List<StoryApplicationModel> storyModels = new List<StoryApplicationModel>();
            foreach (var story in stories)
                storyModels.Add(CreateStoryModelFromStory(db, story));
            return storyModels;
        }

        public static StoryApplicationModel CreateStoryModelFromStory(HourglassContext db, Story story)
        {
            string storyID = story.StoryId;
            List<StoryBlockModel> storyBlocks = getStoryBlocksWithStoryID(db, storyID);
            StoryApplicationModel model = new StoryApplicationModel(story);
            model.StoryBlocks = storyBlocks;
            return model;
        }

        public static List<StoryBlockModel> getStoryBlocksWithStoryID(HourglassContext db, string StoryID)
        {
            List<StoryBlockModel> graphBlocks = GetGraphStoryBlockOnStoryID(db, StoryID);
            List<StoryBlockModel> mapBlocks = GetMapBlocksWithStoryID(db, StoryID);
            List<StoryBlockModel> textBlocks = GetTextBlocksWithStoryID(db, StoryID);
            List<StoryBlockModel> allStories = graphBlocks.Concat(textBlocks)
                                    .Concat(mapBlocks)
                                    .ToList();
            allStories.Sort();
            return allStories;
        }

        //Returns StoryBlocks SORTED by position 
        public static List<StoryBlockModel> GetGraphStoryBlockOnStoryID(HourglassContext db, string StoryID)
        {
            var res = from storyBlock in db.StoryBlock
                      join graphBlock in db.GraphBlock
                          on storyBlock.BlockId equals graphBlock.BlockId
                      where storyBlock.StoryId == StoryID
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
                GraphBlock graphBlock = StoryFactory.createGraphBlock(val.BlockId, val.GraphId);
                storyBlocks.Add(new StoryBlockModel(graphBlock, val.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetMapBlocksWithStoryID(HourglassContext db, string StoryID)
        {
            var res = from storyBlock in db.StoryBlock
                      join geomapBlock in db.GeoMapBlock
                          on storyBlock.BlockId equals geomapBlock.BlockId
                      where storyBlock.StoryId == StoryID
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
                GeoMapBlock geoMapBlock = StoryFactory.createGeoMapBlock(val.BlockId, val.GeoMapId);
                storyBlocks.Add(new StoryBlockModel(geoMapBlock, val.position));
            }
            return storyBlocks;
        }

        public static List<StoryBlockModel> GetTextBlocksWithStoryID(HourglassContext db, string StoryID)
        {
            var res = from storyBlock in db.StoryBlock
                      join geomapBlock in db.TextBlock
                          on storyBlock.BlockId equals geomapBlock.BlockId
                      where storyBlock.StoryId == StoryID
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
                TextBlock textBlock = StoryFactory.createTextBlock(val.BlockId, val.EditorState);
                storyBlocks.Add(new StoryBlockModel(textBlock, val.position));
            }
            return storyBlocks;
        }

    }
}
