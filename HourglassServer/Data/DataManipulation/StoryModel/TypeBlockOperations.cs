using System;
using HourglassServer.Models.Persistent;
using HourglassServer.Data.Application.StoryModel;
using HourglassServer.Data.DataManipulation.DbSetOperations;

namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public class TypeBlockOperations
    {
        public static void MutateTypeBlock(HourglassContext db, StoryBlockModel model, MutatorOperations operation, string storyId)
        {
            StoryBlockType storyType = model.Type;
            switch (storyType)
            {
                case StoryBlockType.TEXTDB:
                    TextBlock textBlock = StoryFactory.CreateTextBlockFromStoryBlockModel(model);
                    DbSetMutator.PerformOperationOnDbSet<TextBlock>(db.TextBlock, operation, textBlock);
                    break;
                case StoryBlockType.GRAPH:
                    GraphBlock graphBlock = StoryFactory.CreateGraphBlockFromStoryBlockModel(model);
                    DbSetMutator.PerformOperationOnDbSet<GraphBlock>(db.GraphBlock, operation, graphBlock);
                    break;
                case StoryBlockType.GEOMAP:
                    GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlockFromStoryBlockModel(model);
                    DbSetMutator.PerformOperationOnDbSet<GeoMapBlock>(db.GeoMapBlock, operation, geoMapBlock);
                    break;
                default:
                    throw new ArgumentException("Could not recognize type of story block: " + storyType);
            }
            StoryBlock storyBlock = StoryFactory.CreateStoryBlockFromStoryBlockModel(model, storyId);
            DbSetMutator.PerformOperationOnDbSet<StoryBlock>(db.StoryBlock, operation, storyBlock);
        }
    }
}
