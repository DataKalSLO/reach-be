using System.ComponentModel.DataAnnotations;

namespace HourglassServer
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        public string Occupation { get; set; }

        [Required]
        public bool NotificationsEnabled { get; set; }
    }
}
