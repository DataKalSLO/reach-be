using HourglassServer.Images;
using Microsoft.AspNetCore.Http;

namespace HourglassServer.Models.Application.ImageModel
{
    public class FormImage
    {
        public IFormFile ImageFile { get; set; }
        public ImageExtensions Type { get; set; }
        public string Id { get; set; }
    }

    public class EncodedImage
    {
        public string ImageEncoded { get; set; }
        public ImageExtensions Type { get; set; }
        public string Id { get; set; }
    }
}
