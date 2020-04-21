using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HourglassServer.Data.BookmarkModel
{
    public class Bookmark
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ItemId { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContentType Type;

        public Bookmark()
        {
        }
    }
}
