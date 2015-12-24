using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Class BestFirstSearch.
    /// </summary>
    public class BestFirstSearch : HeuristicSearch
    {
        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="System.InvalidOperationException">Invalid Heuristic!</exception>
        public override void Add(Node node)
        {
            if (Heuristic == null)
                throw new InvalidOperationException("Invalid Heuristic!");

            var h = Heuristic(node.State);
            Add(node, h);
        }
    }
}
