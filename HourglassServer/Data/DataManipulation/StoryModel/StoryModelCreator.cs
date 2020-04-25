using HourglassServer.Models.Persistent;
using HourglassServer.Data;
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
        public static StoryApplicationModel AddStoryApplicationModelToDatabaseContext(HourglassContext db, StoryApplicationModel storyModel)
        {
            Story newStory = StoryFactory.CreateStoryFromStoryModel(storyModel);
            db.Story.Add(newStory);
            for (int position = 0; position < storyModel.StoryBlocks.Count; position++)
            {
                var storyBlockModel = storyModel.StoryBlocks[position];
                storyBlockModel.BlockPosition = position;
                TypeBlockOperations.PerformOperationOnTypeBlock(db, storyBlockModel, TypeBlockOperations.TypeBlockOperation.ADD, storyModel.Id);
            }
            return storyModel;
        }
    }
}
