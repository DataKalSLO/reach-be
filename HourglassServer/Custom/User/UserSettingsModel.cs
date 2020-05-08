using System.ComponentModel.DataAnnotations;

namespace HourglassServer
{
    public class UserSettingsModel
    {
        public string Name { get; set; }

        public string Occupation { get; set; }

        public bool? NotificationsEnabled { get; set; }
    }
}
