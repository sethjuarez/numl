using System;
using System.Linq;
using System.Reflection;
using numl.Serialization;

namespace numl.Model
{
    public class StringPropertySerializer : PropertySerializer
    {
        public override object Create()
        {
            return new StringProperty();
        }

        public override bool CanConvert(Type type)
        {
            return typeof(StringProperty).IsAssignableFrom(type);
        }

        public override object Read(JsonReader reader)
        {
            var p = (StringProperty)base.Read(reader);
            p.Separator = reader.ReadProperty().Value.ToString();
            p.SplitType = (StringSplitType)Enum.Parse(typeof(StringSplitType),
                                reader.ReadProperty().Value.ToString());
            p.Dictionary = ((object[])reader.ReadArrayProperty().Value)
                                .Select(o => (string)o)
                                .ToArray();
            p.Exclude = ((object[])reader.ReadProperty().Value)
                                .Select(o => (string)o)
                                .ToArray();
            p.AsEnum = (bool)reader.ReadProperty().Value;


            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            base.Write(writer, value);
            var p = (StringProperty)value;
            writer.WriteProperty("Separator", p.Separator);
            writer.WriteProperty("SplitType", Enum.GetName(typeof(StringSplitType), p.SplitType));
            writer.WriteArrayProperty("Dictionary", p.Dictionary);
            writer.WriteArrayProperty("Exclude", p.Exclude);
            writer.WriteProperty("AsEnum", p.AsEnum);
        }
    }
}
