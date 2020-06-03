using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
   public class UniversityInfoUploadModel
   {
      [Required]
      public UniversityInfo[] UniversityInfo;
   }
}
