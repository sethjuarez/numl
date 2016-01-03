using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Class SimpleSearch.
    /// </summary>
    public class SimpleSearch : Search
    {
        private readonly List<IState> _closed;

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        public List<ISuccessor> Solution { get; set; }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public ISearchStrategy Strategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSearch"/> class.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="avoidRepetition">if set to <c>true</c> [avoid repetition].</param>
        public SimpleSearch(ISearchStrategy strategy, bool avoidRepetition = true)
        {
            Strategy = strategy;
            if (avoidRepetition)
                _closed = new List<IState>();
            else
                _closed = null;
        }

        /// <summary>
        /// Finds the specified initial state.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool Find(IState initialState)
        {
            if (Strategy == null) return false;

            Strategy.Add(new Node(initialState));
            while (Strategy.Count() > 0)
            {
                var n = Strategy.Remove();
                if (n.Parent != null && n.Successor != null)
                {
                    var eventArgs = new StateExpansionEventArgs(n.Parent.State, n.Successor, n.Cost, n.Depth);
                    OnSuccessorExpanded(this, eventArgs);

                    if (eventArgs.CancelExpansion)
                        return false;
                }

                if (n.State.IsTerminal)
                {
                    CreateSolution(n);
                    return true;
                }

                foreach (var node in n.Expand(_closed))
                {
                    Strategy.Add(node);
                    if (_closed != null) _closed.Add(node.State);
                }

            }

            return false;
        }

        private void CreateSolution(Node n)
        {
            if (Solution == null) Solution = new List<ISuccessor>();
            var node = n;
            while (!node.IsRoot)
            {
                Solution.Add(node.Successor);
                node = node.Parent;
            }
            Solution.Reverse();
        }
    }
}
