using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class AirportsUploadModel
    {
        [Required]
        public Airports[] Airports;
    }
}
