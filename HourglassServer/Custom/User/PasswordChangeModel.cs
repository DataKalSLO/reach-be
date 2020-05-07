using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.User
{
    public class PasswordChangeModel
    {
        [Required]
        public string Password { get; set; }
    }
}
