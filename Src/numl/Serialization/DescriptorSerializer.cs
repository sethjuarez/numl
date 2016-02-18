using System;
using System.IO;
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Serialization
{
    public class DescriptorSerializer : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(Descriptor).IsAssignableFrom(type);
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                Descriptor d = new Descriptor();
                reader.ReadStartObject();
                d.Name = reader.ReadProperty().Value.ToString();
                //d.Features = reader.ReadNextArrayProperty(new PropertySerializer()).
                //                Value.ToArray<Property>();
                //d.Label = (Property)reader.ReadNextProperty(new PropertySerializer()).Value;
                var typeName = reader.ReadProperty().Value;
                if (typeName != null)
                    d.Type = Ject.FindType(typeName.ToString());
                reader.ReadEndObject();
                return d;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var d = (Descriptor)value;

                writer.WriteStartObject();

                writer.WriteFirstProperty("Name", d.Name);
                writer.WriteNextArrayProperty("Features", d.Features);
                writer.WriteProperty("Label", d.Label);

                writer.WriteProperty("Type", d.Type?.FullName);

                writer.WriteEndObject();
            }
        }
    }
}
