using System;
using System.Reflection;

namespace numl.Serialization
{
    /// <summary>
    /// An abstract JsonSerializer class.
    /// </summary>
    public abstract class JsonSerializer : ISerializer
    {
        internal const string SERIALIZER_ATTRIBUTE = "$serializer";

        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type type);

        /// <summary>
        /// Creates an instance of the type to be deserialized
        /// </summary>
        /// <returns>System.Object.</returns>
        public abstract object Create();


        /// <summary>
        /// Reads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>System.Object.</returns>
        public abstract object Read(JsonReader reader);

        /// <summary>
        /// Reads the closing content.
        /// </summary>
        /// <param name="reader"></param>
        public void PostRead(JsonReader reader)
        {
            reader.ReadEndObject();
        }

        /// <summary>
        /// Writes the opening as well as any static content.
        /// </summary>
        /// <param name="writer"></param>
        public void PreWrite(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteFirstProperty(SERIALIZER_ATTRIBUTE, GetType().FullName);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public abstract void Write(JsonWriter writer, object value);

        /// <summary>
        /// Writes closing content.
        /// </summary>
        /// <param name="writer"></param>
        public void PostWrite(JsonWriter writer)
        {
            writer.WriteEndObject();
        }
    }

    public abstract class JsonSerializer<T> : JsonSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull())
                return null;
            else
            {
                var t = (T)Create();
                foreach (var p in typeof(T).GetTypeInfo().DeclaredProperties)
                {
                    if (p.CanRead && p.CanWrite)
                    {
                        var property = reader.ReadProperty();
                        if (property.Name == p.Name)
                            p.SetValue(t, Convert.ChangeType(property.Value, p.PropertyType));
                    }
                }
                return t;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var t = (T)value;
                foreach (var p in typeof(T).GetProperties())
                {
                    if (p.CanRead && p.CanWrite)
                        writer.WriteProperty(p.Name, p.GetValue(t));
                }
            }
        }
    }
}
