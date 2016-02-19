using System;
using System.IO;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Serialization
{

    public class DateTimePropertySerializer : PropertySerializer
    {
        /// <summary>
        /// Initializes a new instance of the DateTimePropertySerializer class.
        /// </summary>
        public DateTimePropertySerializer()
        {
            Type = typeof(DateTimeProperty);
        }
        public override bool CanConvert(Type type)
        {
            return typeof(DateTimeProperty).IsAssignableFrom(type);
        }

        public override object Read(JsonReader reader)
        {
            var p = (DateTimeProperty)base.Read(reader);

            var features = ((object[])reader.ReadArrayProperty().Value)
                            .Select(o => (string)o)
                            .ToArray();

            p.Features = DateTimeProperty.GetFeatures(features);

            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            base.Write(writer, value);
            var p = (DateTimeProperty)value;
            writer.WriteFirstArrayProperty("Features", DateTimeProperty.GetColumns(p.Features));
        }
        
    }
}
