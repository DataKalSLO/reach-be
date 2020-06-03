using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.Maps
{
public class MultiPolygonGeometry
{
    public string Type { get; set; }
    public IEnumerable<IEnumerable<IEnumerable<IEnumerable<decimal>>>> Coordinates { get; set; }

    private List<List<decimal>> GetCoordinates(List<Point> points)
    {
        List<List<decimal>> coordinateList = new List<List<decimal>>();
        foreach (Point point in points)
        {
            coordinateList.Add(new List<decimal>()
            {
                point.Longitude, point.Latitude
            });
        }
            return coordinateList;
    }
    public MultiPolygonGeometry(List<List<Point>> polygons)
    {
        this.Type = "MultiPolygon";
        List<List<List<decimal>>> coordinateList = new List<List<List<decimal>>>();

        foreach (List<Point> polygon in polygons)
        {
            coordinateList.Add(GetCoordinates(polygon));       
        }
        List<List<List<List<decimal>>>> fullList = new List<List<List<List<decimal>>>>();
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