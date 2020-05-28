using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.User
{
    public class PasswordChangeModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
