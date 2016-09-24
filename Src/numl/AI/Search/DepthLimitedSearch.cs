using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Class DepthLimitedSearch.
    /// </summary>
    public class DepthLimitedSearch : ISearchStrategy
    {
        readonly Stack<Node> _list;
        private readonly int _limit;
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthLimitedSearch"/> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public DepthLimitedSearch(int limit)
        {
            _limit = limit;
            _list = new Stack<Node>();
        }

        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Add(Node node)
        {
            if (node.Depth <= _limit)
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
