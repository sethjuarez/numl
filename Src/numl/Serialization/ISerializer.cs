using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization
{
    /// <summary>
    /// Interface ISerializer
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
        bool CanConvert(Type type);


        /// <summary>
        /// Reads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>System.Object.</returns>
        object Read(JsonReader reader);


        /// <summary>
        /// Cleans up object after reading type
        /// </summary>
        /// <param name="reader">The reader.</param>
        void PostRead(JsonReader reader);

        /// <summary>
        /// Writes out type Json Administrivia
        /// </summary>
        /// <param name="writer">The writer.</param>
        void PreWrite(JsonWriter writer);

        /// <summary>
        /// Writes the specified object
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        void Write(JsonWriter writer, object value);

        /// <summary>
        /// Writes out closing brace etc.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void PostWrite(JsonWriter writer);
    }
}
