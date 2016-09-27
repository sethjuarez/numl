using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI.Search
{
    /// <summary>
    /// Class AlphaBeta.
    /// </summary>
    public class AlphaBeta<TState, TSuccessor> : AdversarialSearch<TState, TSuccessor> where TState : class, IState
                                                                                       where TSuccessor : class, ISuccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaBeta&lt;TState, TSuccessor&gt;"/> class.
        /// </summary>
        public AlphaBeta()
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
                a = Max(Root, double.NegativeInfinity, double.PositiveInfinity);
            else
                a = Min(Root, double.NegativeInfinity, double.PositiveInfinity);

            return a.Successor as TSuccessor;
        }

        /// <summary>
        /// Maximums the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="beta">The beta.</param>
        /// <returns>Node.</returns>
        public Node Max(Node node, double alpha, double beta)
        {
            if (IsTerminal(node)) return node;

            double v = double.NegativeInfinity;

            foreach (var successor in (node.State as IAdversarialState).GetSuccessors())
            {
                if (!ProcessEvent(node, (TSuccessor)successor)) return node;

                var s = successor.State as IAdversarialState;
                var child = new Node(node, (TSuccessor)successor) { Cost = s.Utility, Depth = node.Depth + 1 };
                node.AddChild(child);


                var g = Min(child, alpha, beta);
                v = System.Math.Max(v, g.Cost);
                child.Cost = g.Cost;
                node.Cost = v;

                // check to see if we can prune
                if (v >= beta) return child;
                alpha = System.Math.Max(alpha, v);
            }

            if (node.Children.Count == 0)
                return node;
            else
                return GetBestChildNode(node, v);
        }

        /// <summary>
        /// Minimums the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="beta">The beta.</param>
        /// <returns>Node.</returns>
        public Node Min(Node node, double alpha, double beta)
        {
            if (IsTerminal(node)) return node;

            double v = double.PositiveInfinity;

            foreach (var successor in (node.State as IAdversarialState).GetSuccessors())
            {
                if (!ProcessEvent(node, (TSuccessor)successor)) return node;

                var s = successor.State as IAdversarialState;
                var child = new Node(node, (TSuccessor)successor) { Cost = s.Utility, Depth = node.Depth + 1 };
                node.AddChild(child);

                var g = Max(child, alpha, beta);
                v = System.Math.Min(v, g.Cost);
                child.Cost = g.Cost;
                node.Cost = v;

                // check to see if we can prune
                if (v <= alpha) return child;
                beta = System.Math.Min(beta, v);
            }

            if (node.Children.Count == 0)
                return node;
            else
                return GetBestChildNode(node, v);
        }
    }
}
