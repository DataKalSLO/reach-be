using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Map
{
    public class PolygonFeature
    {
        public string Type { get; set; }
        public PolygonGeometry Geometry { get; set; }
        public Property Property { get; set; }

        public PolygonFeature(List<Point> points, string name)
        {
            this.Type = "Feature";
            this.Geometry = new PolygonGeometry(points);
            this.Property = new Property(name);
        }

        public override string ToString()
        {
            return Type + ", " + "( " + Geometry.ToString() + " )";
        }
       
    }
 }
