using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Application.Maps
{
    public class Properties
    {
        public string Name { get; set; }
        public Dictionary<string, object> Values { get; set; }

        public Properties(string name, Dictionary<string, object> values)
        {
            this.Name = name;
            this.Values = values;
        }
    }
}