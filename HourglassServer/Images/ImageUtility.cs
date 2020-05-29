using System;
using System.IO;

namespace HourglassServer.Images
{
    public class ImageUtility
    {
        public readonly string CONTENT_TYPE_SVG = "image/svg+xml";

        public static string GetContentType(ImageExtensions extension)
        {
            switch (extension)
            {
                case ImageExtensions.SVG:
                    return "image/svg+xml";
                default:
                    return "";
            }
        }

        public static string CreateFileName(string id, ImageExtensions extension)
        {
            return id + "." + extension.ToString().ToLower();
        }

        public static MemoryStream EncodedSvgToStream(string encodedGraphSVG)
        {
            byte[] data = Convert.FromBase64String(encodedGraphSVG);
            return new MemoryStream(data);
        }
    }
}
