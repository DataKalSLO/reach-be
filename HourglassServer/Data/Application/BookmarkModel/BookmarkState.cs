using System;
namespace HourglassServer.Data.Application.BookmarkModel
{
    public class BookmarkState
    {
        public BookmarkState(bool enabled)
        {
            this.Enabled = enabled;
        }

        public bool Enabled { get; set; }
    }
}
