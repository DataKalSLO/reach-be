﻿using System.Collections.Generic;
using System.Linq;

namespace HourglassServer.Data.Map
{
    public class FeatureCollection
    {

        public string Type { get; set; }
        public string Name { get; set; }

        public IEnumerable<IEnumerable<Feature>> FeatureList { get; set; }

        public FeatureCollection() { }

        public FeatureCollection(string tableName, IEnumerable<Feature> features)
        {
            this.Type = "FeatureCollection";
            this.Name = tableName;
            this.FeatureList = features.Select(f =>
            {
                List<Feature> innerList = new List<Feature>();
                innerList.Add(f);
                return innerList;
            });
        }

    }
}
