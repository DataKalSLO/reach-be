using System;
using System.Collections.Generic;
using System.Linq;

namespace HourglassServer.Data.Map
{
    public class PolygonFeatureCollection
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public IEnumerable<IEnumerable<PolygonFeature>> FeatureList { get; set; }

        public PolygonFeatureCollection() { }

        public PolygonFeatureCollection(IEnumerable<PolygonFeature> features)
        {
            this.Type = "PolygonFeatureCollection";
            this.FeatureList = features.Select(f =>
            {
                List<PolygonFeature> innerList = new List<PolygonFeature>();
                innerList.Add(f);
                return innerList;
            });
        }

        public override string ToString()
        {
            string result = "";
            foreach (List<PolygonFeature> feat in FeatureList)
            {
                foreach (PolygonFeature f in feat)
                {
                    result+= (f.ToString());
                }

            }
            return result;
        }
    }
}
