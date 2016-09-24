using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.AI.Functions
{
    /// <summary>
    /// ITransitionFunction interface.
    /// </summary>
    public interface ITransitionFunction
    {
        /// <summary>
        /// Transitions to the new state, given the current state and intended action.
        /// </summary>
        /// <param name="state">Current state.</param>
        /// <param name="action">Intended action.</param>
        /// <returns>IState.</returns>
        IState Transition(IState state, IAction action);
    }
}
