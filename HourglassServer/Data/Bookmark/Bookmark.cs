using System;
using System.ComponentModel.DataAnnotations;

namespace HourglassServer.Data.Bookmark
{
    public class Bookmark
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ItemId { get; set; }

        public ContentType Type;

        public Bookmark()
        {
        }
    }
}
