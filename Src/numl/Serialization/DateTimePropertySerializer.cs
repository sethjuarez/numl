using System;
using System.IO;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Serialization
{

    public class DateTimePropertySerializer : PropertySerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(DateTimeProperty).IsAssignableFrom(type);
        }

        public override Property CreateProperty(Dictionary<string, object> dictionary)
        {
            var features = DateTimeProperty.GetFeatures(dictionary["Features"].ToStringArray());
            DateTimeProperty p = new DateTimeProperty(features);
            p = (DateTimeProperty)MapBaseProperties(p, dictionary);
            return p;

        }

        public override void WriteAdditionalProperties(TextWriter writer, object value)
        {
            var p = (DateTimeProperty)value;
            writer.WriteNextArrayProperty("Features", DateTimeProperty.GetColumns(p.Features));
        }
    }
}
