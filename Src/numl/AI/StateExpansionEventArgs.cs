using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Class StateEventArgs.
    /// </summary>
    public class StateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateEventArgs"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public StateEventArgs(IState state)
        {
            State = state;
        }
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public IState State { get; private set; }
    }
    /// <summary>
    /// Class StateExpansionEventArgs.
    /// </summary>
    public class StateExpansionEventArgs : StateEventArgs
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StateExpansionEventArgs"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="successor">The successor.</param>
        /// <param name="cost">The cost.</param>
        /// <param name="depth">The depth.</param>
        public StateExpansionEventArgs(IState parent, ISuccessor successor, double cost, int depth)
            :base (parent)
        {
            Depth = depth;
            Cost = cost;
            Successor = successor;
            CancelExpansion = false;
                    
        }
        /// <summary>
        /// Gets or sets a value indicating whether [cancel expansion].
        /// </summary>
        /// <value><c>true</c> if [cancel expansion]; otherwise, <c>false</c>.</value>
        public bool CancelExpansion { get; set; }
        /// <summary>
        /// Gets the successor.
        /// </summary>
        /// <value>The successor.</value>
        public ISuccessor Successor { get; private set; }
        /// <summary>
        /// Gets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; private set; }
        /// <summary>
        /// Gets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; private set; }
    }
}
