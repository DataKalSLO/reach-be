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
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.DbSetOperations;
    using HourglassServer.Models.Persistent;

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
                TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, MutatorOperations.ADD, storyModel.Id);
            }

            return storyModel;
        }
    }
}
