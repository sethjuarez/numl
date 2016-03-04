using System;
using numl.Utils;
using System.Linq;
using numl.Math.Normalization;
using System.Collections.Generic;

namespace numl.Serialization.Supervised
{
    /// <summary>
    /// A generic Model serializer object.
    /// </summary>
    public abstract class ModelSerializer : JsonSerializer
    {
        /// <summary>
        /// Deserializes a generic model object from the stream.
        /// </summary>
        /// <param name="reader">A JSON Reader.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Serializes a generic model object to the stream.
        /// </summary>
        /// <param name="writer">A JSON Writer.</param>
        /// <param name="value"></param>
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
