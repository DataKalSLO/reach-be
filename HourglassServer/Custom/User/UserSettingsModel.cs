using System.ComponentModel.DataAnnotations;

namespace HourglassServer
{
    public class UserSettingsModel
    {
        public class PasswordChange
        {
            [Required]
            public string CurrentPassword { get; set; }

            [Required]
            public string NewPassword { get; set; }
        }

        public string Name { get; set; }

        public string Occupation { get; set; }

        public bool? NotificationsEnabled { get; set; }

        public PasswordChange PasswordChangeRequest { get; set; }
    }
}
