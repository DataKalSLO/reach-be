using System;
using System.IO;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphSnapshotOperations
    {
        // Creates a file path in the temp file directory for the svg file
        public static string GetTempFilePath(string graphId) {
            return System.IO.Path.GetTempPath() + graphId + ".svg";
        }

        // Decode the base64 encoded SVG string and write bytes as an SVG file
        public static string WriteSvgFile(string encodedGraphSVG, string path) {
            byte[] data = Convert.FromBase64String(encodedGraphSVG);

            File.WriteAllBytes(path, data);

            return path;
        }

        public static string UploadSnapshotToS3(string path) {
            // TODO: Connect backend with S3 bucket and add upload functionality
            File.Delete(path);

            // Return the URL to the image in S3
            return "NOT_IMPLEMENTED";
        }
    }
}