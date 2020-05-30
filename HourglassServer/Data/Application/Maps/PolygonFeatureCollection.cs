using System;
using System.Collections.Generic;
using System.Linq;

namespace HourglassServer.Data.Application.Maps
{
    public class PolygonFeatureCollection
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public IEnumerable<PolygonFeature> Features { get; set; }

        public PolygonFeatureCollection() { }

        public PolygonFeatureCollection(IEnumerable<PolygonFeature> features)
        {
            this.Type = "PolygonFeatureCollection";
            this.Features = features;
        }

        public override string ToString()
        {
            string result = "";
            foreach (PolygonFeature feat in Features)
            {

             result += (feat.ToString());
                
            }
            return result;
        }
    }
}
