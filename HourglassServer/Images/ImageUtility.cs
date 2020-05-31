using System;
using System.IO;

namespace HourglassServer.Images
{
    public class ImageUtility
    {
        public const string CONTENT_TYPE_JPG = "image/jpeg";
        public const string CONTENT_TYPE_PNG = "image/png";
        public const string CONTENT_TYPE_SVG = "image/svg+xml";

        public static string GetContentType(ImageExtensions extension)
        {
            switch (extension)
            {
                case ImageExtensions.SVG:
                    return CONTENT_TYPE_SVG;
                case ImageExtensions.PNG:
                    return CONTENT_TYPE_PNG;
                case ImageExtensions.JPG:
                    return CONTENT_TYPE_JPG;
                default:
                    return "";
            }
        }

        public static string CreateFileName(string id, ImageExtensions extension)
        {
            return id + "." + extension.ToString().ToLower();
        }

        public static MemoryStream DecodeContentToMemoryStream(string encodedContent)
        {
            byte[] data = Convert.FromBase64String(encodedContent);
            return new MemoryStream(data);
        }
    }
}
