using System;
using HourglassServer.Data.Persistent;
using HourglassServer.Data.Application.StoryModel;

namespace HourglassServer.Data.DataManipulation.StoryModel
{
    public class TypeBlockOperations
    {
        public enum TypeBlockOperation { UPDATE, ADD };

        public static void PerformOperationOnTypeBlock(postgresContext db, StoryBlockModel model, TypeBlockOperation operation)
        {
            StoryBlockType storyType = model.Type;
            switch (storyType)
            {
                case StoryBlockType.TEXT:
                    TextBlock textBlock = StoryFactory.CreateTextBlockFromStoryBlockModel(model);
                    switch (operation)
                    {
                        case TypeBlockOperation.UPDATE:
                            db.TextBlock.Update(textBlock);
                            break;
                        case TypeBlockOperation.ADD:
                            db.TextBlock.Add(textBlock);
                            break;
                    }

                    break;
                case StoryBlockType.GRAPH:
                    GraphBlock graphBlock = StoryFactory.CreateGraphBlockFromStoryBlockModel(model);
                    switch (operation)
                    {
                        case TypeBlockOperation.UPDATE:
                            db.GraphBlock.Update(graphBlock);
                            break;
                        case TypeBlockOperation.ADD:
                            db.GraphBlock.Add(graphBlock);
                            break;
                    }
                    break;
                case StoryBlockType.GEOMAP:
                    GeoMapBlock geoMapBlock = StoryFactory.CreateGeoMapBlockFromStoryBlockModel(model);
                    db.GeoMapBlock.Update(geoMapBlock);
                    switch (operation)
                    {
                        case TypeBlockOperation.UPDATE:
                            db.GeoMapBlock.Update(geoMapBlock);
                            break;
                        case TypeBlockOperation.ADD:
                            db.GeoMapBlock.Add(geoMapBlock);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException("Could not recognize type of story block: " + storyType);
            }
        }
    }
}
