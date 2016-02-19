using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace numl.Serialization
{
    public class EnumerablePropertySerializer : PropertySerializer
    {
        /// <summary>
        /// Initializes a new instance of the EnumerablePropertySerializer class.
        /// </summary>
        public EnumerablePropertySerializer()
        {
            Type = typeof(EnumerableProperty);
        }
        public override bool CanConvert(Type type)
        {
            return typeof(EnumerableProperty).IsAssignableFrom(type);
        }

        public override object Read(JsonReader reader)
        {
            var p = (EnumerableProperty)base.Read(reader);
            var length = int.Parse(reader.ReadProperty().Value.ToString());
            p.Length = length;
            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            base.Write(writer, value);
            var p = (EnumerableProperty)value;
            writer.WriteProperty("Length", p.Length);
        }
    }
}
