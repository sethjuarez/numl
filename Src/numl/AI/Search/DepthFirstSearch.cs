using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Class DepthFirstSearch.
    /// </summary>
    public class DepthFirstSearch : ISearchStrategy
    {
        readonly Stack<Node> _list;
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearch"/> class.
        /// </summary>
        public DepthFirstSearch()
        {
            _list = new Stack<Node>();
        }

        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Add(Node node)
        {
            _list.Push(node);
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int Count()
        {
            return _list.Count;
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        /// <returns>Node.</returns>
        public Node Remove()
        {
            return _list.Pop();
        }
    }
}
