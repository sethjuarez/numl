using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;

namespace numl.Reinforcement.Functions
{
    /// <summary>
    /// IRewardFunction interface.
    /// </summary>
    public interface IRewardFunction
    {
        /// <summary>
        /// Computes the reward for the given state and intended action.
        /// <para>NOTE: The new state (stateP) is encapsulated in <see cref="IState.GetSuccessors"/> w.r.t the <paramref name="action"/>.</para>
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="action">Intended action.</param>
        /// <returns>Double.</returns>
        double Compute(IState state, IAction action);
    }
}
