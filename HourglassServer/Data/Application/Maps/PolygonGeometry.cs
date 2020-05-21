using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
    public class PolygonGeometry
    {
        public string Type { get; set; }
        public IEnumerable<IEnumerable<IEnumerable<decimal>>> Coordinates { get; set; }

        public PolygonGeometry(List<Point> coordinates)
        {
            this.Type = "Polygon";
            List<List<decimal>> coordinateList = new List<List<decimal>>();
            foreach (Point point in coordinates)
            {
                coordinateList.Add(new List<decimal>()
                {
                    point.Longitude, point.Latitude
                });
            }

            List<List<List<decimal>>> fullList = new List<List<List<decimal>>>();
            fullList.Add(coordinateList);
            this.Coordinates = fullList;
        }

        public override string ToString()
        {
            string result = Type + ", [";
            foreach (List<List<decimal>> innerList in Coordinates)
            {
                foreach (List<decimal> coordinates in innerList)
                {
                    result += " ( ";
                    foreach (decimal coord in coordinates)
                    {
                        result += coord + " ";
                    }
                    result += " ) \r";

                }
            }
            return result + "]";
        }
    }
}