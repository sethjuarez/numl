using System;
using System.Reflection;
using numl.Supervised;
using numl.Supervised.NaiveBayes;
using System.Linq;
using numl.Math;

namespace numl.Serialization.Supervised.NaiveBayes
{
    public class NaiveBayesSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(NaiveBayesModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new NaiveBayesModel();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var m = base.Read(reader) as NaiveBayesModel;
                m.Root = reader.ReadProperty().Value as Measure;
                return m;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var m = value as NaiveBayesModel;
                base.Write(writer, m);
                writer.WriteProperty(nameof(m.Root), m.Root);
            }
        }
    }

    public class RangeSerializer : JsonSerializer<Range> { }

    public class MeasureSerializer : JsonSerializer<Measure>
    {
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var measure = (Measure)value;
                writer.WriteProperty(nameof(Measure.Label), measure.Label);
                writer.WriteProperty(nameof(Measure.Discrete), measure.Discrete);
                writer.WriteArrayProperty(nameof(Measure.Probabilities), measure.Probabilities);
            }
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var measure = (Measure)this.Create();

                measure.Label = reader.ReadProperty().Value.ToString();
                measure.Discrete = (bool)reader.ReadProperty().Value;
                var probabilities = reader.ReadArrayProperty().Value;
                if(probabilities != null)
                    measure.Probabilities = probabilities
                                                .Select(o => (Statistic)o)
                                                .ToArray();

                return measure;
            }
        }
    }

    public class StatisticSerializer : JsonSerializer<Statistic>
    {
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var statistic = (Statistic)value;
                writer.WriteProperty(nameof(Statistic.Label), statistic.Label);
                writer.WriteProperty(nameof(Statistic.Discrete), statistic.Discrete);
                writer.WriteProperty(nameof(Statistic.Count), statistic.Count);
                writer.WriteProperty(nameof(Statistic.X), statistic.X);
                writer.WriteProperty(nameof(Statistic.Probability), statistic.Probability);
                writer.WriteArrayProperty(nameof(Statistic.Conditionals), statistic.Conditionals);
            }
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var statistic = (Statistic)this.Create();

                statistic.Label = reader.ReadProperty().Value.ToString();
                statistic.Discrete = (bool)reader.ReadProperty().Value;
                statistic.Count = int.Parse(reader.ReadProperty().Value.ToString());
                statistic.X = (Range)reader.ReadProperty().Value;
                statistic.Probability = (double)reader.ReadProperty().Value;
                var conditionals = reader.ReadArrayProperty().Value;
                if(conditionals != null)
                    statistic.Conditionals = conditionals
                                                .Select(o => (Measure)o)
                                                .ToArray();

                return statistic;
            }
        }
    }
}
