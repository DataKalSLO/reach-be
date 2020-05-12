namespace HourglassServer.Data.DataManipulation.StoryModel
{
    using System;
    using HourglassServer.Data.Application.StoryModel;
    using HourglassServer.Data.DataManipulation.DbSetOperations;
    using HourglassServer.Models.Persistent;

    public class TypeBlockOperations
    {
        public static void MutateTypeBlock(HourglassContext db, StoryBlockModel model, MutatorOperations operation, string storyId)
        {
            StoryBlockType storyType = model.Type;
            switch (storyType)
            {
                case StoryBlockType.TEXTDB:
                    TextBlock textBlock = StoryFactory.CreateTextBlockFromStoryBlockModel(model, storyId);
                    DbSetMutator.PerformOperationOnDbSet<TextBlock>(db.TextBlock, operation, textBlock);
                    break;
                case StoryBlockType.GRAPH:
                    GraphBlock graphBlock = StoryFactory.CreateGraphBlockFromStoryBlockModel(model, storyId);
                    DbSetMutator.PerformOperationOnDbSet<GraphBlock>(db.GraphBlock, operation, graphBlock);
                    break;
                case StoryBlockType.GEOMAP:
                    GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlockFromStoryBlockModel(model, storyId);
                    DbSetMutator.PerformOperationOnDbSet<GeoMapBlock>(db.GeoMapBlock, operation, geoMapBlock);
                    break;
                default:
                    throw new ArgumentException("Could not recognize type of story block: " + storyType);
            }
        }
    }
}
