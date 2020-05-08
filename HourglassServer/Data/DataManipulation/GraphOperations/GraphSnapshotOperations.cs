using System;
using System.IO;

namespace HourglassServer.Data.DataManipulation.GraphOperations
{
    public class GraphSnapshotOperations
    {
        public static string UploadSnapshotToS3(string encodedGraphSVG) 
        {
            string path = GraphSnapshotOperations.GetTempFilePath();

            if (GraphSnapshotOperations.WriteSvgFile(encodedGraphSVG, path)) 
            {
                // TODO: Connect backend with S3 bucket and add upload functionality
                string snapshotURL = "NOT_IMPLEMENTED";

                File.Delete(path);

                // Return the URL to the image in S3
                return snapshotURL;
            }

            return String.Empty;
        }

        // Returns a new random file name in the temp file directory
        private static string GetTempFilePath() 
        {
            return Path.GetTempPath() + Path.GetRandomFileName() + ".svg";
        }

        // Decode the base64 encoded SVG string and write bytes as an SVG file
        private static bool WriteSvgFile(string encodedGraphSVG, string path) 
        {
            try 
            {
                byte[] data = Convert.FromBase64String(encodedGraphSVG);
                File.WriteAllBytes(path, data);
                return true;
            }
            catch (FormatException)
            {
                // TODO: Implement static logging to be able to log the error from this function
                return false;
            }
            catch (Exception)
            {
                // TODO: Implement static logging to be able to log the error from this function
                return false;
            }
        }
    }
}
