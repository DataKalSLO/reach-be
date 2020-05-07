using System;
namespace HourglassServer.Data.Application.Maps
{
    public class Property
    {
        public string Name { get; set; }
        public int? Value { get; set; }

        public Property(string name, int? value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
