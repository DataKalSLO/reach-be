using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
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
