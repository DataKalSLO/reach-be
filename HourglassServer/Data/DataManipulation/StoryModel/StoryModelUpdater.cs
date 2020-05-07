using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using System.Linq;

/* Responsibility: Update any changes to a story
 *
 * Changes handled:
 * - Updating Story meta information
 * - creating story block
 * - removing story block
 * - updating story block
 */
namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public static class StoryModelUpdater
    {
        public static void UpdateStoryApplicationModel(HourglassContext db, StoryApplicationModel storyModel)
        {
            Story storyWithId = StoryFactory.CreateStoryFromStoryModel(storyModel); //Isolates the Story part
            db.Story.Update(storyWithId);

            List<string> storyBlockIdsRequestedToUpdate = new List<String>();
            foreach(StoryBlockModel storyBlockModel in storyModel.StoryBlocks)
                storyBlockIdsRequestedToUpdate.Add(storyBlockModel.Id);

            List<string> storyBlockIdsNoLongerInStory = db.StoryBlock
                .Where(sb => sb.StoryId == storyModel.Id && !storyBlockIdsRequestedToUpdate.Contains(sb.BlockId))
                .Select(storyBlock => storyBlock.BlockId).ToList();

            foreach(string storyBlockId in storyBlockIdsNoLongerInStory)
                DeleteStoryBlockModelByBlockId(db, storyBlockId);

            UpdateExistingOrAddNewStoryBlocks(db, storyModel.StoryBlocks, storyModel.Id);
        }

        private static void UpdateExistingOrAddNewStoryBlocks(HourglassContext db, List<StoryBlockModel> storyBlocks, string storyId)
        {
            for (int blockPosition = 0; blockPosition < storyBlocks.Count; blockPosition++)
            {
                StoryBlockModel storyBlockModel = storyBlocks[blockPosition];
                storyBlockModel.BlockPosition = blockPosition;
                if (db.StoryBlock.Any(sb => sb.BlockId == storyBlockModel.Id))
                    TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.UPDATE, storyId);
                else
                    TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.ADD, storyId);
            }
        }

        //TODO: Merge into `StoryModelDeleter.cs` when exists (coming soon to PRs near you).
        private static void DeleteStoryBlockModelByBlockId(HourglassContext db, string storyBlockId)
        {
            // Note, I do not store the type of a story block.
            // Therefore, I must check to see where it exists.
            GraphBlock graphBlock = db.GraphBlock.Find(storyBlockId);
            GeoMapBlock geoMapBlock = db.GeoMapBlock.Find(storyBlockId);
            TextBlock textBlock = db.TextBlock.Find(storyBlockId);

            if (graphBlock != null)
                db.GraphBlock.Remove(graphBlock);
            else if (geoMapBlock != null)
                db.GeoMapBlock.Remove(geoMapBlock);
            else if (textBlock != null)
                db.TextBlock.Remove(textBlock);

            StoryBlock storyBlock = db.StoryBlock.Where(storyBlock => storyBlock.BlockId == storyBlockId).First();
            db.StoryBlock.Remove(storyBlock);
        }
    }
}
