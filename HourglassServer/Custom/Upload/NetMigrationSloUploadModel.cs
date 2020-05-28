using HourglassServer.Models.Persistent;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.Upload
{
    public class NetMigrationSloUploadModel
    {
        [Required]
        public NetMigrationSlo[] NetMigrationSlo;
    }
}
