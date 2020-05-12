// <copyright file="BookmarkApplicationModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HourglassServer.Data.Application.BookmarkModel
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public enum ContentType { GRAPH, STORY, GEOMAP }

    public class BookmarkApplicationModel
    {
        public BookmarkApplicationModel() { }

        public string UserId { get; set; }

        public string ContentId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] // Strings not converted by default
        public ContentType ContentType { get; set; }
    }
}
