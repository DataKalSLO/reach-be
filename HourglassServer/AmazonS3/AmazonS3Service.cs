using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace HourglassServer.AmazonS3
{
    public class AmazonS3Service
    {
        public static AmazonS3Client GetClient(string accessKey, string accessSecret, RegionEndpoint region)
        {
            return new AmazonS3Client(accessKey, accessSecret, region);
        }

        public static async Task<string> UploadObject(AmazonS3Client client,
                MemoryStream stream, string region, string bucket, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = fileName,
                InputStream = stream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = await client.PutObjectAsync(request);

            var objectUrl = AmazonUtility.GetObjectUrl(bucket, region, fileName);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return objectUrl;
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> RemoveObject(AmazonS3Client client, string bucket, string fileName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };

            var response = await client.DeleteObjectAsync(request);

            return (response.HttpStatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}