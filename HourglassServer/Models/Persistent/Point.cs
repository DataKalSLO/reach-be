﻿using System;
using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Point
    {
        public Point()
        {
            Area = new HashSet<Area>();
            Location = new HashSet<Location>();
        }

        public int Id { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public virtual ICollection<Area> Area { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
