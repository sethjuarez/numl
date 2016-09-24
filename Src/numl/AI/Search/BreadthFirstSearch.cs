using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Class BreadthFirstSearch.
    /// </summary>
    public class BreadthFirstSearch : ISearchStrategy
    {
        readonly Queue<Node> _list;
        /// <summary>
        /// Initializes a new instance of the <see cref="BreadthFirstSearch"/> class.
        /// </summary>
        public BreadthFirstSearch()
        {
            _list = new Queue<Node>();
        }

        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Add(Node node)
        {
            _list.Enqueue(node);
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
            return _list.Dequeue();
        }
    }
}