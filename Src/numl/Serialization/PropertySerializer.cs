using numl.Model;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization
{
    public class PropertySerializer : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(Property).IsAssignableFrom(type);
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


            return new Property
            {
                Name = name,
                Type = type,
                Start = start,
                Discrete = discrete,
            };
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
