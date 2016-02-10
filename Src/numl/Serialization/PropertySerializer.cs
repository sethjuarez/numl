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
    public class PropertySerializer : ISerializer
    {
        public virtual bool CanConvert(Type type)
        {
            return typeof(Property).IsAssignableFrom(type);
        }

        public object Read(TextReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                reader.ReadStartObject();
                var s = Ject.FindType(reader.ReadProperty().Value.ToString());

                var serializer = (PropertySerializer)Activator.CreateInstance(s);

                var name = reader.ReadNextProperty().Value.ToString();

                Type type = null;
                var typeName = reader.ReadNextProperty().Value;
                if (typeName != null)
                    type = Ject.FindType(typeName.ToString());

                var start = int.Parse(reader.ReadNextProperty().Value.ToString());
                var discrete = (bool)reader.ReadNextProperty().Value;

                var property = serializer.ReadAdditionalProperties(reader);

                reader.ReadEndObject();

                property.Name = name;
                property.Type = type;
                property.Start = start;
                property.Discrete = discrete;

                return property;
            }
        }


        public void Write(TextWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var p = (Property)value;

                writer.WriteStartObject();
                writer.WriteProperty("Serializer", GetType().FullName);
                writer.WriteNextProperty("Name", p.Name);
                writer.WriteNextProperty("Type", p.Type?.FullName);
                writer.WriteNextProperty("Start", p.Start);
                writer.WriteNextProperty("Discrete", p.Discrete);

                WriteAdditionalProperties(writer, value);

                writer.WriteEndObject();
            }
        }

        public virtual Property ReadAdditionalProperties(TextReader reader)
        {
            Property p = new Property();
            return p;
        }

        public virtual void WriteAdditionalProperties(TextWriter writer, object value)
        {

        }
    }
}
