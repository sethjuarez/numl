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

        public object Deserialize(TextReader reader)
        {
            var o = Serializer.Read(reader);
            if (o is Dictionary<string, object>)
            {
                var dictionary = (Dictionary<string, object>)o;
                return CreateProperty(dictionary);
            }
            else
                throw new InvalidOperationException("Invalid Parse :(");

        }


        public void Write(TextWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var p = (Property)value;

                writer.WriteStartObject();
                writer.WriteProperty("Serializer", this.GetType().FullName);
                writer.WriteNextProperty("Name", p.Name);
                writer.WriteNextProperty("Type", p.Type.FullName);
                writer.WriteNextProperty("Start", p.Start);
                writer.WriteNextProperty("Discrete", p.Discrete);

                WriteAdditionalProperties(writer, value);

                writer.WriteEndObject();
            }
        }

        public Property MapBaseProperties(Property property, Dictionary<string, object> dictionary)
        {
            property.Name = dictionary["Name"].ToString();
            property.Type = Ject.FindType(dictionary["Type"].ToString());
            property.Start = int.Parse(dictionary["Start"].ToString());
            property.Discrete = (bool)dictionary["Discrete"];
            return property;
        }
        public virtual Property CreateProperty(Dictionary<string, object> dictionary)
        {
            Property property = new Property();
            return MapBaseProperties(property, dictionary);
        }

        public virtual void WriteAdditionalProperties(TextWriter writer, object value)
        {

        }
    }
}
