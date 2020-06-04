using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HourglassServer.Data.Application.Maps
{
    public class Feature
    {
        public string Type { get; set; }
        public JObject Geometry { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public Feature(string jsonString, Dictionary<string, object> values)
        {
            this.Type = Enum.GetName(typeof(MapType), MapType.Feature);
            this.Geometry = JObject.Parse(jsonString);
            this.Properties = values;
        }

        public override string ToString()
        {
            return Type + ", " + Geometry.ToString() + ",";
        }
    }
}