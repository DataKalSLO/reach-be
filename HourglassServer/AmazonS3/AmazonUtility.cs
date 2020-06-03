using Amazon;
using Microsoft.Extensions.Configuration;

namespace HourglassServer.AmazonS3
{
    public static class AmazonUtility
    {
        // Get the region endpoint from the string representation ("us-east-2") from appsettings
        public static RegionEndpoint GetAWSRegionEndpoint(this IConfiguration config)
        {
            string region = config.GetSection("AWS")["Region"];
            return RegionEndpoint.GetBySystemName(region);
        }

        // Get the AWS access key from appsettings
        public static string GetAWSAccessKey(this IConfiguration config)
        {
            return config.GetSection("AWS")["AccessKey"];
        }

        // Get the AWS secret key from appsettings
        public static string GetAWSSecretKey(this IConfiguration config)
        {
            return config.GetSection("AWS")["AccessSecret"];
        }

        // Get the AWS S3 bucket name for a specified bucket from appsettings
        public static string GetAWSBucket(this IConfiguration config, Bucket bucket)
        {
            switch (bucket)
            {
                case Bucket.GRAPH_SNAPSHOT:
                    return config.GetSection("AWS:Buckets")["GraphSnapshot"];
                case Bucket.STORY_IMAGE_BLOCK:
                    return config.GetSection("AWS:Buckets")["StoryImageBlock"];
                default:
                    return null;
            }
        }

        // Object URLs are created in S3 based on the bucket name, region, and file name
        public static string GetObjectUrl(string bucket, string region, string fileName)
        {
            string url = "https://" + bucket + ".s3." + region + ".amazonaws.com/" + fileName;

            return url;
        }
    }
}