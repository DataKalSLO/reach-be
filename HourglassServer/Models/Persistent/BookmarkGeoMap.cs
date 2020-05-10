using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class BookmarkGeoMap
    {
        public string UserId { get; set; }
        public string GeoMapId { get; set; }

        public virtual GeoMap GeoMap { get; set; }
    }
}
