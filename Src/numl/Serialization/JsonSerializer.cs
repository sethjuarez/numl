using System;
using System.Collections.Generic;
using System.Linq;

namespace numl.Serialization
{
    public abstract class JsonSerializer : ISerializer
    {
        internal const string SERIALIZER_ATTRIBUTE = "$Serializer";

        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type type);


        /// <summary>
        /// Reads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>System.Object.</returns>
        public abstract object Read(JsonReader reader);

        public void PostRead(JsonReader reader)
        {
            reader.ReadEndObject();
        }


        public void PreWrite(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteFirstProperty(SERIALIZER_ATTRIBUTE, GetType().FullName);
        }

        /// <summary>
        /// Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public abstract void Write(JsonWriter writer, object value);


        public void PostWrite(JsonWriter writer)
        {
            writer.WriteEndObject();
        }
    }
}
