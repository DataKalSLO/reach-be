using System;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class Feature
    {
        public string Type { get; set; }
        public PointGeometry Geometry { get; set; }
        public Property Property { get; set; }

        public Feature(Location location)
        {
            this.Type = "Feature";
            this.Geometry = new PointGeometry(location.Point.Longitude, location.Point.Latitude);
            this.Property = new Property(location.Name);
        }

        public override string ToString()
        {
            return Type + ", " + Geometry.ToString() + ", " + Property.Name;
        }
    }
}
