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
        public bool CanConvert(Type type)
        {
            return typeof(StringProperty).IsAssignableFrom(type);
        }
        
        //public override Property ReadAdditionalProperties(TextReader reader)
        //{
        //    StringProperty p = new StringProperty();
        //    p.Separator = reader.ReadNextProperty().Value.ToString();
        //    p.SplitType = (StringSplitType)Enum.Parse(typeof(StringSplitType), 
        //                        reader.ReadNextProperty().Value.ToString());
        //    p.Dictionary = ((object[])reader.ReadNextArrayProperty().Value).ToStringArray();
        //    p.Exclude = ((object[])reader.ReadNextArrayProperty().Value).ToStringArray();
        //    p.AsEnum = (bool)reader.ReadNextProperty().Value;

        //    return p;
        //}

        //public override void WriteAdditionalProperties(TextWriter writer, object value)
        //{
        //    var p = (StringProperty)value;
        //    writer.WriteNextProperty("Separator", p.Separator);
        //    writer.WriteNextProperty("SplitType", Enum.GetName(typeof(StringSplitType), p.SplitType));
        //    writer.WriteNextArrayProperty("Dictionary", p.Dictionary);
        //    writer.WriteNextArrayProperty("Exclude", p.Exclude);
        //    writer.WriteNextProperty("AsEnum", p.AsEnum);
        //}
    }
}
