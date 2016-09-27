using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.AI
{
    /// <summary>
    /// IAction interface.
    /// </summary>
    public interface IAction : Data.IEdge, IComparable
    {
        /// <summary>
        /// Gets the identifier of the action.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates the probability of this action occuring, i.e. the stochasticity of the action.
        /// </summary>
        double Probability { get; }
    }
}
