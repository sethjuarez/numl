using System;
using numl.Serialization;
using System.Reflection;

namespace numl.Model
{
    public class EnumerablePropertySerializer : PropertySerializer
    {
        public override object Create()
        {
            return new EnumerableProperty();
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
