using System;
using numl.Utils;
using System.Linq;
using numl.Math.Normalization;
using System.Collections.Generic;

namespace numl.Serialization.Supervised
{
    public abstract class ModelSerializer : JsonSerializer
    {
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var model = (numl.Supervised.Model)Create();
                model.Descriptor = (numl.Model.Descriptor)reader.ReadProperty().Value;
                model.NormalizeFeatures = (bool)reader.ReadProperty().Value;

                var normalizer = Ject.FindType(reader.ReadProperty().Value.ToString());
                if(normalizer != null)
                    model.Normalizer = (INormalizer)Activator.CreateInstance(normalizer);

                model.Summary = (Math.Summary)reader.ReadProperty().Value;

                return model;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var model = (numl.Supervised.Model)value;
                writer.WriteProperty("Descriptor", model.Descriptor);
                writer.WriteProperty("NormalizeFeatures", model.NormalizeFeatures);
                writer.WriteProperty("Normalizer", model.Normalizer?.GetType().FullName);
                writer.WriteProperty("Summary", model.Summary);
            }
        }
    }
}
