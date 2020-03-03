using System.ComponentModel.DataAnnotations;

namespace HourglassServer
{
    public class TokenModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}