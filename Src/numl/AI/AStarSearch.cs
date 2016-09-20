using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Class AStarSearch.
    /// </summary>
    public class AStarSearch : HeuristicSearch
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

            var h = node.Cost + Heuristic(node.State);
            Add(node, h);
        }
    }
}
