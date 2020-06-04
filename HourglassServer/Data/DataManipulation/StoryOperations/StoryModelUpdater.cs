/* Responsibility: Update any changes to a story
 *
 * Changes handled:
 * - Updating Story meta information
 * - creating story block
 * - removing story block
 * - updating story block
 */
namespace HourglassServer.Data.DataManipulation.StoryOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Models.Persistent;
    using Microsoft.EntityFrameworkCore;

    public static class StoryModelUpdater
    {
        public static void UpdateStoryApplicationModel(HourglassContext db, StoryApplicationModel storyModel)
        {
            //Referential Entities
            ClearFeedbackOnReviewSubmission(db, storyModel);

            //Story
            Story storyWithId = StoryFactory.CreateStoryFromStoryModel(storyModel); // Isolates the Story part
            storyModel.DateLastEdited = StoryFactory.GetNow();
            storyWithId.DateLastEdited = storyModel.DateLastEdited;
            db.Story.Update(storyWithId);

            //Story Blocks
            List<string> storyBlockIdsRequestedToUpdate = new List<string>();
            foreach (StoryBlockModel storyBlockModel in storyModel.StoryBlocks)
            {
                storyBlockIdsRequestedToUpdate.Add(storyBlockModel.Id);
            }

            List<GraphBlock> graphBlocksNoLongerInStory = db.GraphBlock
                .Where(graphBlock => graphBlock.StoryId == storyModel.Id && !storyBlockIdsRequestedToUpdate.Contains(graphBlock.BlockId)).ToList();
            db.GraphBlock.RemoveRange(graphBlocksNoLongerInStory);

            List<GeoMapBlock> geoMapsBlocksNoLongerInStory = db.GeoMapBlock
               .Where(geoMapBlock => geoMapBlock.StoryId == storyModel.Id && !storyBlockIdsRequestedToUpdate.Contains(geoMapBlock.BlockId)).ToList();
            db.GeoMapBlock.RemoveRange(geoMapsBlocksNoLongerInStory);

            List<TextBlock> textBlocksNoLongerInStory = db.TextBlock
               .Where(textBlock => textBlock.StoryId == storyModel.Id && !storyBlockIdsRequestedToUpdate.Contains(textBlock.BlockId)).ToList();
            db.TextBlock.RemoveRange(textBlocksNoLongerInStory);

            List<ImageBlock> imageBlocksNoLongerInStory = db.ImageBlock
              .Where(imageBlock => imageBlock.StoryId == storyModel.Id &&
              !storyBlockIdsRequestedToUpdate.Contains(imageBlock.BlockId)).ToList();
            db.ImageBlock.RemoveRange(imageBlocksNoLongerInStory);

            UpdateExistingOrAddNewStoryBlocks(db, storyModel.StoryBlocks, storyModel.Id);
        }

        private static void UpdateExistingOrAddNewStoryBlocks(HourglassContext db, List<StoryBlockModel> storyBlocks, string storyId)
        {
            for (int blockPosition = 0; blockPosition < storyBlocks.Count; blockPosition++)
            {
                StoryBlockModel storyBlockModel = storyBlocks[blockPosition];
                storyBlockModel.BlockPosition = blockPosition;
                if (TypeBlockExists(db, storyBlockModel, storyBlockModel.Id))
                {
                    TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.UPDATE, storyId);
                }
                else
                {
                    TypeBlockOperations.MutateTypeBlock(db, storyBlockModel, DbSetOperations.MutatorOperations.ADD, storyId);
                }
            }
        }

        //can't be generalized because you need a lambda that knows the type it receives
        private static bool TypeBlockExists(HourglassContext db, StoryBlockModel storyBlock, string blockId)
        {
            switch (storyBlock.Type)
            {
                case StoryBlockType.TEXTDB:
                    return db.TextBlock.Any(textBlock => textBlock.BlockId == blockId);
                case StoryBlockType.GRAPH:
                    return db.GraphBlock.Any(graphBlock => graphBlock.BlockId == blockId);
                case StoryBlockType.GEOMAP:
                    return db.GeoMapBlock.Any(geoMapBlock => geoMapBlock.BlockId == blockId);
                case StoryBlockType.IMAGE:
                    return db.ImageBlock.Any(imageBlock => imageBlock.BlockId == blockId);
                default:
                    throw new ArgumentException("Could not recognize type of story block: " + storyBlock.Type);
            }
        }

        public static void ClearFeedbackOnReviewSubmission(HourglassContext db, StoryApplicationModel storyModel)
        {
            //Referential Entities
            Story existingStory = db.Story.AsNoTracking().Where(Story => Story.StoryId == storyModel.Id).First();
                PublicationStatus oldPublicationStatus = StoryFactory.GetPublicationStatus(existingStory);
            if (oldPublicationStatus != storyModel.PublicationStatus &&
                storyModel.PublicationStatus == PublicationStatus.REVIEW)
            {
                IList<StoryFeedback> feedbackToRemove = db.StoryFeedback
                    .Where(storyFeedback => storyFeedback.StoryId == storyModel.Id)
                    .ToList();
                db.StoryFeedback.RemoveRange(feedbackToRemove);
            }
        }
    }
}
