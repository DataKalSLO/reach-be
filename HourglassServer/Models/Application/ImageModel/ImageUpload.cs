using HourglassServer.Images;
using Microsoft.AspNetCore.Http;

namespace HourglassServer.Models.Application.ImageModel
{
    /* 
     * Represent an image submitted as a part of a form.
     */
    public class FormImage
    {
        public IFormFile ImageFile { get; set; }
        public ImageExtensions Type { get; set; }
        public string Id { get; set; }
    }

    /*
     * Represents an image submitted as an encoded string.
     */
    public class EncodedImage
    {
        public string ImageEncoded { get; set; }
        public ImageExtensions Type { get; set; }
        public string Id { get; set; }
    }
}
