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
            Descriptor d = new Descriptor();
            d.Name = reader.ReadProperty().Value.ToString();
            d.Features = ((object[])reader.ReadArrayProperty().Value)
                            .Select(o => (Property)o)
                            .ToArray();
            d.Label = (Property)reader.ReadProperty().Value;
            var typeName = reader.ReadProperty().Value;
            if (typeName != null)
                d.Type = Ject.FindType(typeName.ToString());
            return d;

        }

        public override void Write(JsonWriter writer, object value)
        {
            var d = (Descriptor)value;
            writer.WriteProperty("Name", d.Name);
            writer.WriteArrayProperty("Features", d.Features);
            writer.WriteProperty("Label", d.Label);
            writer.WriteProperty("Type", d.Type?.FullName);
        }
    }
}
