using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HourglassServer.Data.BookmarkModel
{
    public class BookmarkContent
    {
        [JsonConverter(typeof(StringEnumConverter))] //Strings not converted by default
        public ContentType Type;
        public object Content;

        public BookmarkContent()
        {
        }
    }
}
