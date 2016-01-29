using System;
using System.Linq;
using System.Reflection;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Serialization
{
    /// <summary>
    /// MatrixConverter
    /// </summary>
    public class MatrixConverter 
    {
        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType">type to convert</param>
        /// <returns>can convert</returns>
        public  bool CanConvert(Type objectType)
        {
            return typeof(Matrix).IsAssignableFrom(objectType);
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
        //    if (reader.TokenType == JsonToken.Null) return null;

        //    return new Matrix(serializer.Deserialize<double[][]>(reader));
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
        //        serializer.Serialize(writer, ((Matrix)value).ToArray());
        //}
    }
}
