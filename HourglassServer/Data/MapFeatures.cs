using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HourglassServer.Data
{
    public class MapFeatures
    {
        public Properties properties { get; set; } = new Properties();
        public Geometry geometry { get; set; } = new Geometry();
    }

    public class Properties
    {
        public int CollegeID { get; set; }
        public string Name { get; set; }
        public int NumDegrees { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }
}
