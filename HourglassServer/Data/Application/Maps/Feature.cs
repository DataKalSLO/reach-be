using System;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class Feature
    {
        public string Type { get; set; }
        public PointGeometry Geometry { get; set; }
        public Properties Properties { get; set; }

        public Feature(PointGeometry point, string name, int? value)
        {
            this.Type = "Feature";
            this.Geometry = point;
            this.Properties = new Properties(name, value);
        }

        public override string ToString()
        {
            return Type + ", " + Geometry.ToString() + ", " + Properties.Name;
        }
    }
}