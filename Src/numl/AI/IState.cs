using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Interface IState
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is terminal.
        /// </summary>
        /// <value><c>true</c> if this instance is terminal; otherwise, <c>false</c>.</value>
        bool IsTerminal { get; }
        /// <summary>
        /// Determines whether [is equal to] [the specified state].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if [is equal to] [the specified state]; otherwise, <c>false</c>.</returns>
        bool IsEqualTo(IState state);
        /// <summary>
        /// Gets the successors.
        /// </summary>
        /// <returns>IEnumerable&lt;ISuccessor&gt;.</returns>
        IEnumerable<ISuccessor> GetSuccessors();
        /// <summary>
        /// Heuristics this instance.
        /// </summary>
        /// <returns>System.Double.</returns>
        double Heuristic();
    }
}