using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class ZipArea
    {
        public int Zip { get; set; }
        public string Area { get; set; }

        public virtual Area AreaNavigation { get; set; }
        public virtual ZipCode ZipNavigation { get; set; }
    }
}
