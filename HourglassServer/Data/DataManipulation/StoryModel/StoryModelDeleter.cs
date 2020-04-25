using System.Linq;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;

// Responsibility: Delete all entities associated with a Story
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelDeleter
    {
        /* Note, I was unable to implement Foreign Key constraints on my persistent models
         * leading to no native cascading delete operation. However, postgres does have this
         * compability which I utilize below to delete all related entities after deleting a Story.
         */
        public static string DeleteStoryByID(HourglassContext db, string storyId)
        {
            StoryApplicationModel storyApplicationModelToDelete = StoryModelRetriever.GetStoryApplicationModelById(db, storyId);
            Story storyToDelete = StoryFactory.CreateStoryFromStoryModel(storyApplicationModelToDelete);
            db.Remove(storyToDelete);

            foreach (StoryBlockModel storyBlockModel in storyApplicationModelToDelete.StoryBlocks)
                TypeBlockOperations.PerformOperationOnTypeBlock(db, storyBlockModel, TypeBlockOperations.TypeBlockOperation.DELETE, storyId);
            return storyId;
        }
    }
}
