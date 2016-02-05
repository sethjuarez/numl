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
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="reader">The stream.</param>
        /// <returns>System.Object.</returns>
        object Deserialize(TextReader reader);
        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="writer">The stream.</param>
        /// <param name="value">The o.</param>
        void Write(TextWriter writer, object value);
    }
}
