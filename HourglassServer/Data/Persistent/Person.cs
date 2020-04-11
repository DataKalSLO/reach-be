using System;
using System.Collections.Generic;

namespace HourglassServer.Data.Persistent
{
    public partial class Person
    {
        public Person()
        {
            Story = new HashSet<Story>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<Story> Story { get; set; }
    }
}
