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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Tree)
            {
                if (!base.Equals((Graph)obj))
                    return false;
                else if (!((Tree)obj).Root.Equals(Root))
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
    }
}
