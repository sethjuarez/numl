using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Math;
using numl.Math.Normalization;
using numl.Math.LinearAlgebra;
using AutoTensor;
using numl.Supervised;

namespace numl.Reinforcement
{
    /// <summary>
    /// IReinforcementModel interface.
    /// </summary>
    public interface IReinforcementModel : IModel
    {
        /// <summary>
        /// Reinforces the model from the new state, action and reward.
        /// </summary>
        /// <param name="x">Item features, i.e. the State.</param>
        /// <param name="y">Action label.</param>
        /// <param name="r">Reward value.</param>
        void Learn(Vector x, double y, double r);

        /// <summary>
        /// Reinforces the model from the new State, Action, StateP and Reward.
        /// </summary>
        /// <param name="x1">Item features, i.e. the State.</param>
        /// <param name="y">Label or Action.</param>
        /// <param name="x2">Transition state features, i.e. the new State.</param>
        /// <param name="r">Reward value.</param>
        void Learn(Vector x1, double y, Vector x2, double r);
    }
}
