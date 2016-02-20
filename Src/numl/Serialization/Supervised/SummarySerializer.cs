using numl.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization.Supervised
{
    public class SummarySerialize : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(Summary).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new Summary();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var summary = (Summary)Create();
                summary.Average = reader.ReadVector();
                summary.Minimum = reader.ReadVector();
                summary.Median = reader.ReadVector();
                summary.Maximum = reader.ReadVector();
                summary.StandardDeviation = reader.ReadVector();
                return summary;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var summary = (Summary)value;
                writer.WriteProperty("Average", summary.Average);
                writer.WriteProperty("Minimum", summary.Minimum);
                writer.WriteProperty("Median", summary.Median);
                writer.WriteProperty("Maximum", summary.Maximum);
                writer.WriteProperty("StandardDeviation", summary.StandardDeviation);
            }
        }
    }
}
