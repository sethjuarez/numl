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
    /// JsonProperty structure.
    /// </summary>
    public struct JsonProperty
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public object Value { get; set; }
    }
}
