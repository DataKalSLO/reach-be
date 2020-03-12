using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Map
{
    public class PolygonFeature
    {
        public string Type { get; set; }
        public PolygonGeometry Geometry { get; set; }

        public PolygonFeature(List<Point> points)
        {
            this.Type = "Feature";
            this.Geometry = new PolygonGeometry(points);
        }

        public override string ToString()
        {
            return Type + ", " + Geometry.ToString();
        }
       
    }
 }
