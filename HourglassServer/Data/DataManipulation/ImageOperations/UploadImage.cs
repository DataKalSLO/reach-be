using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HourglassServer.AmazonS3;
using HourglassServer.Images;
using System.IO;
using Microsoft.AspNetCore.Http;
using Amazon;
using Amazon.S3;
using HourglassServer.Models.Application.ImageModel;

namespace HourglassServer.Data.DataManipulation.ImageOperations
{
    public class ImageManipulator
    {
        /*
         * Upload Images
         */

        public static async Task<string> UploadFormFileToBucket(
            FormImage formImage,
            IConfiguration config,
            Bucket bucketType
        )
        {
            MemoryStream stream = new MemoryStream();
            formImage.ImageFile.CopyTo(stream);
            var contentType = ImageUtility.GetContentType(formImage.Type);
            var fileName = ImageUtility.CreateFileName(formImage.Id, formImage.Type);
            return await UploadMemoryStream(config, bucketType, fileName, stream, contentType);
        }

        public static async Task<string> UploadEncodedImage(
            EncodedImage encodedImage,
            IConfiguration config,
            Bucket bucketType
        )
        {
            string contentType = ImageUtility.GetContentType(encodedImage.Type);
            string fileName = ImageUtility.CreateFileName(encodedImage.Id, encodedImage.Type);
            MemoryStream stream = ImageUtility.DecodeContentToMemoryStream(encodedImage.ImageEncoded);
            return await UploadMemoryStream(config, bucketType, fileName, stream, contentType);
        }

        private static async Task<string> UploadMemoryStream(
            IConfiguration config,
            Bucket bucketType,
            string fileName,
            MemoryStream stream,
            string contentType
        )
        {
            string bucket = config.GetAWSBucket(bucketType);
            RegionEndpoint region = config.GetAWSRegionEndpoint();
            AmazonS3Client client = GetAmazonClient(config);
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

        /*
         * Delete Operations
         */

        public static async Task RemoveImageFromS3(
            IConfiguration config,
            Bucket bucketType,
            ImageExtensions imageType,
            string imageId
        )
        {
            var fileName = ImageUtility.CreateFileName(imageId, imageType);
            await RemoveImageFromS3(config, bucketType, fileName);
        }

        public static async Task RemoveImageFromS3(
            IConfiguration config,
            Bucket bucketType,
            string fileName
        )
        {
            string bucket = config.GetAWSBucket(bucketType);
            AmazonS3Client client = GetAmazonClient(config);
            await AmazonS3Service.RemoveObject(client, bucket, fileName);
        }

        /*
         * Private Helper Methods
         */

        private static AmazonS3Client GetAmazonClient(IConfiguration config)
        {
            RegionEndpoint region = config.GetAWSRegionEndpoint();

            AmazonS3Client client = AmazonS3Service.GetClient(
                config.GetAWSAccessKey(),
                config.GetAWSSecretKey(),
                region
            );

            return client;
        }
    }
}
