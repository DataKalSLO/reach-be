using System.Collections.Generic;

namespace HourglassServer.Models.Persistent
{
    public partial class Person
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public virtual IEnumerable<Story> Story { get; set; }
   }
}
