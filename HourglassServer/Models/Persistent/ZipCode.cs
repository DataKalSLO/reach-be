using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class ZipCode
    {
        public ZipCode()
        {
            ZipArea = new HashSet<ZipArea>();
        }

        public int Zip { get; set; }
        public int[] Coordinates { get; set; }

        public virtual ICollection<ZipArea> ZipArea { get; set; }
    }
}
