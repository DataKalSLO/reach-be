using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HourglassServer.AmazonS3;
using HourglassServer.Images;
using HourglassServer.Data.Application.GraphModel;
using HourglassServer.Data.DataManipulation.ImageOperations;
using HourglassServer.Models.Application.ImageModel;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphSnapshotOperations
    {
        public static async Task<string> UploadSnapshotToS3(IConfiguration config, GraphModel graphModel)
        {
            EncodedImage image = new EncodedImage()
            {
                ImageEncoded = graphModel.GraphSVG,
                Type = ImageExtensions.SVG,
                Id = graphModel.GraphId
            };
            return await ImageManipulator.UploadEncodedImage(
                image,
                config,
                Bucket.GRAPH_SNAPSHOT
                );
        }

        public static async Task RemoveSnapshotFromS3(IConfiguration config, string graphId)
        {
            await ImageManipulator.RemoveImageFromS3(
                config,
                Bucket.GRAPH_SNAPSHOT,
                ImageExtensions.SVG,
                graphId
                );
        }
    }
}
