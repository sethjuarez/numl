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
        public override bool CanConvert(Type type)
        {
            return typeof(EnumerableProperty).IsAssignableFrom(type);
        }

        public override Property CreateProperty(Dictionary<string, object> dictionary)
        {
            int length = int.Parse(dictionary["Length"].ToString());
            EnumerableProperty p = new EnumerableProperty(length);
            return MapBaseProperties(p, dictionary);
        }

        public override void WriteAdditionalProperties(TextWriter writer, object value)
        {
            var p = (EnumerableProperty)value;
            writer.WriteNextProperty("Length", p.Length);
        }
    }
}
