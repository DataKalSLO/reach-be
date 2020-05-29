using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
   public class CommuteTimesUploadModel
   {
      [Required]
      public CommuteTimes[] CommuteTimes;  
   }
}
