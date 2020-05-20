using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class CovidUnemploymentUploadModel
    {
        [Required]
        public CovidUnemployment[] CovidUnemployment;
    }
}
