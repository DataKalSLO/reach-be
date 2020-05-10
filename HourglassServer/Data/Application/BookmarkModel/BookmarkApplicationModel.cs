using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HourglassServer.Data.Application.BookmarkModel
{

    public enum ContentType { GRAPH, STORY, GEOMAP }

    public class BookmarkApplicationModel
    {
        public BookmarkApplicationModel() { }

        public string UserId { get; set; }
        public string ContentId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] //Strings not converted by default
        public ContentType ContentType { get; set; }
    }
}
