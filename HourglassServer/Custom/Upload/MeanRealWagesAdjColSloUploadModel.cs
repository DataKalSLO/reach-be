using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
   public class MeanRealWagesAdjColSloUploadModel
   {
      [Required]
      public MeanRealWagesAdjColSlo[] MeanRealWagesAdjColSlo;
   }
}
