using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Class Minimax.
    /// </summary>
    public class Minimax<TState, TSuccessor> : AdversarialSearch<TState, TSuccessor> where TState : class, IState
                                                                                     where TSuccessor : class, ISuccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Minimax&lt;TState, TSuccessor&gt;"/> class.
        /// </summary>
        public Minimax()
        {
            Depth = 3;    
        }

        /// <summary>
        /// Finds the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>ISuccessor.</returns>
        public override TSuccessor Find(IAdversarialState state)
        {
            Root = new Node(state);
            Node a;

            if (state.Player)
                a = Max(Root);
            else
                a = Min(Root);

            return (TSuccessor)a.Successor;
        }

        /// <summary>
        /// Finds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="initial">The initial.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="f">The f.</param>
        /// <returns>Node.</returns>
        private Node Find(Node node, double initial, Func<double, double, double> limit, Func<Node, Node> f)
        {
            if (IsTerminal(node)) return node;

            double v = initial;

            foreach (var successor in (node.State as IAdversarialState).GetSuccessors())
            {
                if (!ProcessEvent(node, (TSuccessor)successor)) return node;

                var s = successor.State as IAdversarialState;
                var child = new Node(node, successor) { Cost = s.Utility, Depth = node.Depth + 1 };
                node.AddChild(child);

                var g = f(child);
                v = limit(v, g.Cost);
                child.Cost = g.Cost;
                node.Cost = v;
            }

            if (node.Children.Count == 0)
                return node;
            else
                return GetBestChildNode(node, v);
        }

        /// <summary>
        /// Maximums the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Node.</returns>
        public virtual Node Max(Node node)
        {
            return Find(node, double.NegativeInfinity, System.Math.Max, Min);
        }

        /// <summary>
        /// Minimums the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Node.</returns>
        public virtual Node Min(Node node)
        {
            return Find(node, double.PositiveInfinity, System.Math.Min, Max);
        }
    }
}
