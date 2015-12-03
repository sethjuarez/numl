using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using numl.Utils;

namespace numl.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Type).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
                return Ject.FindType((string)reader.Value);

            throw new JsonSerializationException("Unexpected type property");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                if("".GetType().GetTypeInfo().Assembly == ((Type)value).GetTypeInfo().Assembly)
                    writer.WriteValue(((Type)value).FullName);
                else //TODO: Find a way to not have this anymore...
                    writer.WriteValue(((Type)value).AssemblyQualifiedName);
            }

        }
    }
}
