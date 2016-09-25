using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Interface ISearchStrategy
    /// </summary>
    public interface ISearchStrategy
    {
        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        void Add(Node node);
        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int Count();
        /// <summary>
        /// Removes this instance.
        /// </summary>
        /// <returns>Node.</returns>
        Node Remove();
    }
}
