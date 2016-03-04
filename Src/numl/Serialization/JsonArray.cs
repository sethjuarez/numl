using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using numl.Utils;
using System.Collections;
namespace numl.Serialization
{
    /// <summary>
    /// JsonArray structure.
    /// </summary>
    public struct JsonArray
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public object[] Value { get; set; }
    }
}
