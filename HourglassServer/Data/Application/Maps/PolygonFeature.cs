using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;
using Newtonsoft.Json.Linq;

namespace HourglassServer.Data.Application.Maps
{
    public class PolygonFeature
    {
        public string Type { get; set; }
        public JObject Geometry { get; set; }
        public Properties Properties { get; set; }

        public PolygonFeature(string jsonString, string name, int? value)
        {
            this.Type = "Feature"; 
            this.Geometry = JObject.Parse(jsonString);
            this.Properties = new Properties(name, value);
        }

        public override string ToString()
        {
            return Type + ", " + "( " + Geometry.ToString() + " )";
        }

    }
}