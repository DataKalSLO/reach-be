using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class SloAirportsUploadModel
    {
        [Required]
        public SloAirport[] SloAirports;
    }
}
