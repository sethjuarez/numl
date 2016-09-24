using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.AI.Functions
{
    /// <summary>
    /// Default Heuristic function.
    /// </summary>
    public class Heuristic : IHeuristicFunction
    {
        /// <summary>
        /// Computes the default heuristic value for the given state.
        /// </summary>
        /// <param name="state">Current state.</param>
        /// <returns>Double.</returns>
        public double Compute(IState state)
        {
            return state.Heuristic();
        }
    }
}
