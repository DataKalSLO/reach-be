using HourglassServer.Data.Persistent;
using HourglassServer.Data.Application.StoryModel;

/* Responsibility: Map creation actions from the StoryModel (application) to Story
 * (persistent/database).
 *
 * Mapping:
 *
 * StoryModel       -> Story + StoryBlockModel[]
 * StoryBlockModel  -> StoryBlock + [GeoMapBlock | TextBlock | GraphBlock]                            
 */
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public class StoryModelCreator
    {
        public static StoryApplicationModel AddStoryModelToDatabase(postgresContext db, StoryApplicationModel storyModel)
        {
            AddStoryToDatabase(db, storyModel);
            for (int position = 0; position < storyModel.StoryBlocks.Count; position++)
            {
                var storyBlockModel = storyModel.StoryBlocks[position];
                storyBlockModel.BlockPosition = position;
                AddStoryBlockToDatabase(db, storyBlockModel, storyModel.Id);
            }
            return storyModel;
        }

        private static void AddStoryToDatabase(postgresContext db, StoryApplicationModel model)
        {
            Story newStory = StoryFactory.CreateStoryFromStoryModel(model);
            db.Story.Add(newStory);
        }

        private static void AddStoryBlockToDatabase(postgresContext db, StoryBlockModel blockModel, string storyid)
        {
            StoryBlock newStoryBlock = StoryFactory.CreateStoryBlockFromStoryBlockModel(blockModel, storyid);
            db.StoryBlock.Add(newStoryBlock);
            TypeBlockOperations.PerformOperationOnTypeBlock(db, blockModel, TypeBlockOperations.TypeBlockOperation.ADD);
        }
    }
}
