using System;
namespace HourglassServer.Data.Bookmark
{
    public class Bookmark
    {
        public string UserId { get; set; }
        public string ItemId { get; set; }
        public ContentType Type;

        public Bookmark()
        {
        }
    }
}
