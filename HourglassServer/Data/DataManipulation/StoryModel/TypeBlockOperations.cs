using System;
using HourglassServer.Models.Persistent;
using HourglassServer.Data;
using HourglassServer.Data.Application.StoryModel;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public class TypeBlockOperations
    {
        public enum TypeBlockOperation { UPDATE, ADD, DELETE };

        public static void PerformOperationOnTypeBlock(HourglassContext db, StoryBlockModel model, TypeBlockOperation operation)
        {
            StoryBlockType storyType = model.Type;
            switch (storyType)
            {
                case StoryBlockType.TEXTDB:
                    TextBlock textBlock = StoryFactory.CreateTextBlockFromStoryBlockModel(model);
                    PerformOperationOnDbSet<TextBlock>(db.TextBlock, operation, textBlock);
                    break;
                case StoryBlockType.GRAPH:
                    GraphBlock graphBlock = StoryFactory.CreateGraphBlockFromStoryBlockModel(model);
                    PerformOperationOnDbSet<GraphBlock>(db.GraphBlock, operation, graphBlock);
                    break;
                case StoryBlockType.GEOMAP:
                    GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlockFromStoryBlockModel(model);
                    PerformOperationOnDbSet<GeoMapBlock>(db.GeoMapBlock, operation, geoMapBlock);
                    break;
                default:
                    throw new ArgumentException("Could not recognize type of story block: " + storyType);
            }
        }

        public static void PerformOperationOnDbSet<T>(DbSet<T> dbSet, TypeBlockOperation operation, T model) where T : class
        {
            switch (operation)
            {
                case TypeBlockOperation.UPDATE:
                    dbSet.Update(model);
                    break;
                case TypeBlockOperation.ADD:
                    dbSet.Add(model);
                    break;
                case TypeBlockOperation.DELETE:
                    dbSet.Remove(model);
                    break;
                default:
                    throw new ArgumentException("Could not recognize operation: " + operation.ToString());
            }
        }
    }
}
