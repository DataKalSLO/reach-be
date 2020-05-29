using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HourglassServer.AmazonS3;
using HourglassServer.Images;
using HourglassServer.Data.Application.GraphModel;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphSnapshotOperations
    {
        public static async Task<string> UploadSnapshotToS3(IConfiguration config, GraphModel graphModel)
        {
            var contentType = ImageUtility.GetContentType(ImageExtensions.SVG);
            var fileName = ImageUtility.CreateFileName(graphModel.GraphId, ImageExtensions.SVG);
            var stream = ImageUtility.EncodedSvgToStream(graphModel.GraphSVG);
            var bucket = config.GetAWSBucket(Bucket.GRAPH_SNAPSHOT);
            var region = config.GetAWSRegionEndpoint();

            var client = AmazonS3Service.GetClient(
                config.GetAWSAccessKey(),
                config.GetAWSSecretKey(),
                region
            );

            await AmazonS3Service.RemoveObject(client, bucket, fileName);

            return await AmazonS3Service.UploadObject(
                client,
                stream,
                region.SystemName,
                bucket,
                fileName,
                contentType
            );
        }

        public static async Task RemoveSnapshotFromS3(IConfiguration config, string graphId)
        {
            var fileName = ImageUtility.CreateFileName(graphId, ImageExtensions.SVG);
            var bucket = config.GetAWSBucket(Bucket.GRAPH_SNAPSHOT);
            var region = config.GetAWSRegionEndpoint();

            var client = AmazonS3Service.GetClient(
                config.GetAWSAccessKey(),
                config.GetAWSSecretKey(),
                region
            );

            await AmazonS3Service.RemoveObject(client, bucket, fileName);
        }
    }
}
