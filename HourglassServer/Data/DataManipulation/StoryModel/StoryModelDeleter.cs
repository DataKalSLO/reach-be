using System.Linq;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using System.Collections.Generic;

// Responsibility: Delete all entities associated with a Story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelDeleter
    {
        public static void DeleteStoryById(HourglassContext db, string storyId)
        {
            List<StoryBlockModel> storyBlocks = StoryModelRetriever.GetStoryBlocksByStoryId(db, storyId);            
            foreach (StoryBlockModel storyBlockModel in storyBlocks)
                TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.DELETE, storyId);
            db.Story.Remove(db.Story.Find(storyId));
        }
    }
}
