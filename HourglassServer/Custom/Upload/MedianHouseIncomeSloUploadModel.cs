using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
   public class MedianHouseIncomeSloUploadModel
   {
      [Required]
      public MedianHouseIncomeSlo[] MedianHouseIncomeSlo;
   }
}
