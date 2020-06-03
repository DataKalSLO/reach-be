using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class IncomeInequalitySloUploadModel
    {
        [Required]
        public IncomeInequalitySlo[] IncomeInequalitySlo;
    }
}
