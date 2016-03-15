using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    /// <summary>
    /// Class Tree.
    /// </summary>
    /// <seealso cref="numl.Data.Graph" />
    public class Tree : Graph
    {
        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>The root.</value>
        public IVertex Root { get; set; }
    }
}
