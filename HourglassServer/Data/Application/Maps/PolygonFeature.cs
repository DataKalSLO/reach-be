using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class PolygonFeature
    {
        public string Type { get; set; }
        public PolygonGeometry Geometry { get; set; }
        public Properties Properties { get; set; }

        public PolygonFeature(List<Point> points, string name, int? value)
        {
            this.Type = "Feature";
            this.Geometry = new PolygonGeometry(points);
            this.Properties = new Properties(name, value);
        }

        public override string ToString()
        {
            return Type + ", " + "( " + Geometry.ToString() + " )";
        }

    }
}