using System;
using numl.Model;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;

namespace numl.Serialization.Model
{
    public class PropertySerializer : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(Property).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new Property();
        }

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
            writer.WriteProperty("Name", p.Name);
            writer.WriteProperty("Type", p.Type?.FullName);
            writer.WriteProperty("Start", p.Start);
            writer.WriteProperty("Discrete", p.Discrete);
        }
    }
}
