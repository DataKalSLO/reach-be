using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data.Application.GraphModel
{
    public class GraphApplicationModel
    {
        public string GraphId { get; set; }
        public string UserId { get; set; }
        public long TimeStamp { get; set; }
        public string GraphTitle { get; set; }
        public string SnapshotUrl { get; set; }

        [JsonConverter(typeof(GraphSourceModel))]
        public GraphSource[] DataSources { get; set; }

        public string GraphOptions { get; set; }
    }

    // Custom Serializer for GraphSource for it to exclude nested attributes
    // and only serialize the string attributes
    public class GraphSourceConverter : JsonConverter<GraphSource>
    {
        public override void WriteJson(JsonWriter writer, GraphSource value, JsonSerializer serializer)
        {
            Console.WriteLine("Im here!");
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var properties = value.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(string[]));

            writer.WriteStartObject();

            foreach (var property in properties)
            {
                writer.WritePropertyName(property.Name);
                serializer.Serialize(writer, property.GetValue(value, null));
            }

            writer.WriteEndObject();
        }

        public override GraphSource ReadJson(
                JsonReader reader,
                Type objectType,
                GraphSource existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
        {
            // Only support serialization with this converter
            throw new NotImplementedException();
        }
    }

}
