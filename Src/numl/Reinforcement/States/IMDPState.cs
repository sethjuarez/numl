using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.Math.LinearAlgebra;

namespace numl.Reinforcement
{
    /// <summary>
    /// IMDPState interface.
    /// </summary>
    public interface IMDPState : IState
    {
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        Vector Features { get; set; }
    }
}
