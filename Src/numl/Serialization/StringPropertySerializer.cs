using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Model;
using numl.Utils;

namespace numl.Serialization
{
    public class StringPropertySerializer : PropertySerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(StringProperty).IsAssignableFrom(type);
        }

        public override Property CreateProperty(Dictionary<string, object> dictionary)
        {
            StringProperty property = new StringProperty();
            property = (StringProperty)MapBaseProperties(property, dictionary);

            property.Separator = dictionary["Separator"].ToString();

            property.SplitType = (StringSplitType)Enum.Parse(typeof(StringSplitType), dictionary["SplitType"].ToString());
            property.Dictionary = ((object[])dictionary["Dictionary"]).ForEach(o => o.ToString()).ToArray();
            property.Exclude = ((object[])dictionary["Exclude"]).ForEach(o => o.ToString()).ToArray();
            property.AsEnum = (bool)dictionary["AsEnum"];

            return property;
        }

        public override void WriteAdditionalProperties(TextWriter writer, object value)
        {
            var p = (StringProperty)value;
            writer.WriteNextProperty("Separator", p.Separator);
            writer.WriteNextProperty("SplitType", Enum.GetName(typeof(StringSplitType), p.SplitType));
            writer.WriteNextArrayProperty("Dictionary", p.Dictionary);
            writer.WriteNextArrayProperty("Exclude", p.Exclude);
            writer.WriteNextProperty("AsEnum", p.AsEnum);
        }
    }
}
