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
        
        public override Property ReadAdditionalProperties(TextReader reader)
        {
            var features = reader.ReadNextArrayProperty().Value.ToStringArray();
            var p = new DateTimeProperty(DateTimeProperty.GetFeatures(features));
            return p;
        }

        public override void WriteAdditionalProperties(TextWriter writer, object value)
        {
            var p = (DateTimeProperty)value;
            writer.WriteNextArrayProperty("Features", DateTimeProperty.GetColumns(p.Features));
        }
    }
}
