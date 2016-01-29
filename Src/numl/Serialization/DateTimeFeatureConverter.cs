using System;
using numl.Model;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace numl.Serialization
{
    /// <summary>
    /// JsonConverter for DateTimeFeature
    /// </summary>
    public class DateTimeFeatureConverter 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public  bool CanConvert(Type objectType)
        {
            return typeof(DateTimeFeature).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    if (reader.TokenType == JsonToken.Null)
        //        return null;
        //    else
        //        return DateTimeProperty.GetFeatures(serializer.Deserialize<string[]>(reader));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    if (value == null)
        //        writer.WriteNull();
        //    else
        //        serializer.Serialize(writer, DateTimeProperty.GetColumns((DateTimeFeature)value).ToArray());
        //}
    }
}
