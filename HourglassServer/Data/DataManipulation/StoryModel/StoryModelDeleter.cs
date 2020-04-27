using System.Linq;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;

// Responsibility: Delete all entities associated with a Story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelDeleter
    {
        public static string DeleteStoryByID(HourglassContext db, string storyId)
        {
            StoryApplicationModel storyApplicationModelToDelete = StoryModelRetriever.GetStoryApplicationModelById(db, storyId);
            Story storyToDelete = StoryFactory.ExtractPersistentStoryFromApplicationStory(storyApplicationModelToDelete);
            db.Story.Remove(storyToDelete);
            foreach (StoryBlockModel storyBlockModel in storyApplicationModelToDelete.StoryBlocks)
                TypeBlockOperations.PerformOperationOnTypeBlock(db, storyBlockModel, TypeBlockOperations.TypeBlockOperation.DELETE, storyId);
            return storyId;
        }
    }
}
