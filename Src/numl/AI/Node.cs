using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace numl.AI
{
    /// <summary>
    /// Class Node.
    /// </summary>
    public class Node
    {
        private readonly StateComparer _stateComparer;
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public Node(IState state)
        {
            State = state;
            Parent = null;
            Successor = null;
            Cost = 0;
            Depth = 0;
            Path = false;
            Children = new List<Node>();
            _stateComparer = new StateComparer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="successor">The successor.</param>
        public Node(Node parent, ISuccessor successor)
        {
            State = successor.State;
            Parent = parent;
            Successor = successor;
            Cost = parent.Cost + successor.Cost;
            Depth = parent.Depth + 1;
            Path = false;
            Children = new List<Node>();
            _stateComparer = new StateComparer();
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public Node Parent { get; set; }
        /// <summary>
        /// Gets or sets the successor.
        /// </summary>
        /// <value>The successor.</value>
        public ISuccessor Successor { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public IState State { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public bool IsRoot { get { return Parent == null && Successor == null; } }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Node"/> is path.
        /// </summary>
        /// <value><c>true</c> if path; otherwise, <c>false</c>.</value>
        public bool Path { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; set; }
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<Node> Children { get; set; }

        /// <summary>
        /// Expands this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;Node&gt;.</returns>
        public IEnumerable<Node> Expand() { return Expand(null); }

        /// <summary>
        /// Expands the specified closed.
        /// </summary>
        /// <param name="closed">The closed.</param>
        /// <returns>IEnumerable&lt;Node&gt;.</returns>
        public IEnumerable<Node> Expand(IEnumerable<IState> closed)
        {
            foreach (var successor in State.GetSuccessors())
                if (closed == null || !closed.Contains(successor.State, _stateComparer))
                    AddChild(new Node(this, successor));

            return Children;
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <exception cref="System.InvalidOperationException">Invalid node state!</exception>
        public void AddChild(Node n)
        {
            if (State == null)
                throw new InvalidOperationException("Invalid node state!");

            Children.Add(n);
        }
    }
}
