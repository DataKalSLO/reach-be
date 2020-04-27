using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;

// Responsibility: Update any sub-part of a story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelUpdater
    {
        public static StoryApplicationModel UpdateStoryApplicationModel(HourglassContext db, StoryApplicationModel storyModel)
        {
            Story storyWithId = StoryFactory.ExtractPersistentStoryFromApplicationStory(storyModel); //Isolates the Story part
            db.Story.Update(storyWithId);
            List<string> storyBlockIds = new List<string>();
            for(int position=0;position<storyModel.StoryBlocks.Count; position++)
            {
                var storyBlockModel = storyModel.StoryBlocks[position];
                storyBlockIds.Add(storyBlockModel.Id);
                TypeBlockOperations.PerformOperationOnTypeBlock(db, storyBlockModel, TypeBlockOperations.TypeBlockOperation.UPDATE, storyModel.Id);
            }
            //TODO: Query storyblocks and gather BlockIds missing in given storyModel
            //TODO: Call delete on StoryBlockIds above
            return storyModel;
        }
    }
}
