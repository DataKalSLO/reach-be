using System.Collections.Generic;

namespace HourglassServer.Data.Map
{
    public class PointGeometry
    {
        public string Type { get; set; }
        public IEnumerable<decimal> Coordinates { get; set; }

        public PointGeometry(decimal lng, decimal lat)
        {
            this.Type = "Point";
            List<decimal> coordinates = new List<decimal>();
            coordinates.Add(lng);
            coordinates.Add(lat);
            this.Coordinates = coordinates;
        }

        public override string ToString()
        {
            string result = Type + ", (";
            foreach (float num in Coordinates)
            {
                result += num + " ";
            }
            return result + ")";
        }
    }
}