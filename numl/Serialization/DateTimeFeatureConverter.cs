using System;
using numl.Model;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace numl.Serialization
{
    public class DateTimeFeatureConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeFeature);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            return DateTimeProperty.GetFeatures(serializer.Deserialize<string[]>(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var items = (DateTimeFeature)value;
                serializer.Serialize(writer, DateTimeProperty.GetColumns(items).ToArray());
            }
        }
    }
}
