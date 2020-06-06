/* Responsibility: Map creation actions from the StoryModel (application) to Story
 * (persistent/database).
 *
 * Mapping:
 *
 * StoryModel       -> Story + StoryBlockModel[]
 * StoryBlockModel  -> StoryBlock + [GeoMapBlock | TextBlock | GraphBlock]
 */
namespace HourglassServer.Data.DataManipulation.StoryOperations
{
    using System;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.DbSetOperations;
    using HourglassServer.Models.Persistent;

    public class StoryModelCreator
    {
        public static StoryApplicationModel AddStoryApplicationModelToDatabaseContext(HourglassContext db, StoryApplicationModel storyModel)
        {
            DateTime now = StoryFactory.GetNow();
            Story newStory = StoryFactory.CreateStoryFromStoryModel(storyModel);
            newStory.DateCreated = now;
            newStory.DateLastEdited = now;
            db.Story.Add(newStory);

            for (int position = 0; position < storyModel.StoryBlocks.Count; position++)
            {
                var storyBlockModel = storyModel.StoryBlocks[position];
                storyBlockModel.BlockPosition = position;
                TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, MutatorOperations.ADD, storyModel.Id);
            }

            //cleanup
            storyModel.DateCreated = now;
            storyModel.DateLastEdited = now;

            return storyModel;
        }
    }
}
