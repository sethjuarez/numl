using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;

namespace numl.Reinforcement
{
    /// <summary>
    /// IMDPSuccessor interface.
    /// </summary>
    public interface IMDPSuccessor : ISuccessor
    {
        /// <summary>
        /// Gets the Reward for the transition state.
        /// </summary>
        double Reward { get; }
    }
}
