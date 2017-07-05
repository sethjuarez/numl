using System;
using numl.Utils;
using System.Linq;
using numl.Serialization;
using System.Collections.Generic;

namespace numl.Model
{
    public class DescriptorSerializer : JsonSerializer<Descriptor>
    {
        public override object Read(JsonReader reader)
        {
            Descriptor d = (Descriptor)Create();
            d.Name = reader.ReadProperty().Value.ToString();
            d.Features = reader.ReadArrayProperty().Value
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
            writer.WriteProperty(nameof(Descriptor.Name), d.Name);
            writer.WriteArrayProperty(nameof(Descriptor.Features), d.Features);
            writer.WriteProperty(nameof(Descriptor.Label), d.Label);
            writer.WriteProperty(nameof(Descriptor.Type), d.Type?.FullName);
        }
    }
}
