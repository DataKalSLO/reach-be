using System;
using System.Collections.Generic;

namespace HourglassServer.Data
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
