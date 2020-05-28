using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Custom.User
{
    public class EmailModel
    {
        [Required]
        public string Email { get; set; }
    }
}
