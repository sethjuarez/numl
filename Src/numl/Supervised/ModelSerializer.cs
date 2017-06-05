using System;
using numl.Utils;
using System.Linq;
using numl.Serialization;
using numl.Math.Normalization;
using System.Collections.Generic;

namespace numl.Supervised
{
    /// <summary>
    /// A generic Model serializer object.
    /// </summary>
    public abstract class ModelSerializer : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return false;
        }

        public override object Create()
        {
            return null;
        }

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
                    model.FeatureNormalizer = (INormalizer)Activator.CreateInstance(normalizer);

                model.FeatureProperties = (Math.Summary)reader.ReadProperty().Value;

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
                writer.WriteProperty(nameof(model.Descriptor), model.Descriptor);
                writer.WriteProperty(nameof(model.NormalizeFeatures), model.NormalizeFeatures);
                writer.WriteProperty(nameof(model.FeatureNormalizer), model.FeatureNormalizer?.GetType().FullName);
                writer.WriteProperty(nameof(model.FeatureProperties), model.FeatureProperties);
            }
        }
    }
}
