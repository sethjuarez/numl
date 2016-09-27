using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.AI.Functions
{
    /// <summary>
    /// ISelectionFunction interface.
    /// </summary>
    public interface ISelectionFunction
    {
        /// <summary>
        /// Selects the optimal action for the current state.
        /// </summary>
        /// <param name="state">State instance.</param>
        /// <param name="heuristicFunction">Heuristic to use when selecting the successor state.</param>
        /// <returns>IAction.</returns>
        IAction Select(IState state, IHeuristicFunction heuristicFunction);
    }
}