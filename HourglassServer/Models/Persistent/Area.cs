using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Area
    {
        public Area()
        {
            ZipArea = new HashSet<ZipArea>();
        }

        public string Name { get; set; }
        public int[] Coordinates { get; set; }

        public virtual ICollection<ZipArea> ZipArea { get; set; }
    }
}
