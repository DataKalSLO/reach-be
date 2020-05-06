using System.Linq;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;

// Responsibility: Delete all entities associated with a Story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelDeleter
    {
        public static void DeleteStoryByID(HourglassContext db, string storyId)
        {
            StoryApplicationModel storyApplicationModelToDelete = StoryModelRetriever.GetStoryApplicationModelById(db, storyId);            
            foreach (StoryBlockModel storyBlockModel in storyApplicationModelToDelete.StoryBlocks)
                TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.DELETE, storyId);
            db.Story.Remove(db.Story.Find(storyId));
        }
    }
}
