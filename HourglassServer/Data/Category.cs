using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class Category
    {
        public Category()
        {
            Storycategory = new HashSet<Storycategory>();
        }

        public string Categoryname { get; set; }
        public string Categorydescription { get; set; }

        public virtual ICollection<Storycategory> Storycategory { get; set; }
    }
}
