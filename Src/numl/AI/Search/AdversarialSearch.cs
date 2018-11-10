using System;
using System.Linq;
using System.Collections.Generic;
using numl.Utils;

namespace numl.AI.Search
{
    /// <summary>
    /// Class AdversarialSearch.
    /// </summary>
    public abstract class AdversarialSearch<TState, TSuccessor> : SearchBase<TState> where TState : class, IState
                                                                                     where TSuccessor : class, ISuccessor
    {
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }
        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>The root.</value>
        public Node Root { get; set; }

        internal Node GetBestChildNode(Node node, double v)
        {
            var q = node.Children
                        .Where(n => n.Cost == v && n.State.IsTerminal);

            Node r;
            if (q.Any()) // favor terminal nodes first
                r = q.Random();
            else
                r = node.Children
                            .Where(n => n.Cost == v)
                            .Random();
            r.Cost = v;
            r.Path = true;
            return r;
        }

        internal bool IsTerminal(Node node)
        {
            return node.State.IsTerminal || node.Depth == Depth * 2;
        }

        internal bool ProcessEvent(Node parentNode, TSuccessor successor)
        {
            var state = parentNode.State as TState;
            var args = new StateExpansionEventArgs(state, successor, parentNode.Cost, parentNode.Depth);
            OnSuccessorExpanded(this, args);
            return !args.CancelExpansion;
        }

        /// <summary>
        /// Finds the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>ISuccessor.</returns>
        public abstract TSuccessor Find(IAdversarialState state);
    }
}
