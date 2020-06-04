using System;
using System.Collections.Generic;
using System.Linq;

namespace HourglassServer.Data.Application.Maps
{
    public class PolygonFeatureCollection
    {
        private List<PolygonFeature> features;

        public string Type { get; set; }
        public string Name { get; set; }

        public List<object> Features { get; set; }

        public PolygonFeatureCollection() { }

        public PolygonFeatureCollection(List<object> features)
        {
            this.Type = Enum.GetName(typeof(MapType), MapType.PolygonFeatureCollection);
            this.Features = features;
        }

        public PolygonFeatureCollection(List<PolygonFeature> features)
        {
            this.features = features;
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
