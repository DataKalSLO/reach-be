using System;
namespace HourglassServer.Data.Application.Maps
{
    public class Properties
    {
        public string Name { get; set; }
        public int? Value { get; set; }

        public Properties(string name, int? value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}