using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class MultiPolygonFeature
    {
        public string Type { get; set; }
        public MultiPolygonGeometry Geometry { get; set; }
        public Properties Properties { get; set; }

        public MultiPolygonFeature(List<List<Point>> polygons, string name, int? value)
        {
            this.Type = "Feature";
            this.Geometry = new MultiPolygonGeometry(polygons);
            this.Properties = new Properties(name, value);
        }

        public override string ToString()
        {
            return Type + ", " + "( " + Geometry.ToString() + " )";
        }

    }
}