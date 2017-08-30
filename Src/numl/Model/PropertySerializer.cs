using System;
using numl.Serialization;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;

namespace numl.Model
{
    public class PropertySerializer : JsonSerializer<Property>
    {
        public override object Read(JsonReader reader)
        {
            var name = reader.ReadProperty().Value.ToString();

            Type type = null;
            var typeName = reader.ReadProperty().Value;
            if (typeName != null)
                type = Ject.FindType(typeName.ToString());

            var start = int.Parse(reader.ReadProperty().Value.ToString());
            var discrete = (bool)reader.ReadProperty().Value;

            var p = (Property)Create();
            p.Name = name;
            p.Type = type;
            p.Start = start;
            p.Discrete = discrete;
            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            var p = (Property)value;
            writer.WriteProperty(nameof(p.Name), p.Name);
            writer.WriteProperty(nameof(p.Type), p.Type?.FullName);
            writer.WriteProperty(nameof(p.Start), p.Start);
            writer.WriteProperty(nameof(p.Discrete), p.Discrete);
        }
    }
}
