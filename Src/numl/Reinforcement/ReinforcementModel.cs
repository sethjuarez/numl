using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Math;
using numl.Math.LinearAlgebra;
using numl.Math.Normalization;
using numl.Model;
using numl.Utils;

namespace numl.Reinforcement
{
    /// <summary>
    /// Reinforcement model.
    /// </summary>
    public abstract class ReinforcementModel : Supervised.Model, IReinforcementModel
    {
        /// <summary>
        /// Gets or sets the transition/reward descriptor.
        /// </summary>
        public Descriptor TransitionDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the feature discretizer to use for reducing each item.
        /// </summary>
        public numl.Math.Discretization.IDiscretizer FeatureDiscretizer { get; set; }

        /// <summary>
        /// Reinforces the model from the new item and reward.
        /// </summary>
        /// <param name="state">Initial State object or features with action label.</param>
        /// <param name="stateP">New State object or features with reward label.</param>
        public void Learn(object state, object stateP)
        {
            var doubles1 = Descriptor.Convert(state, true);
            var tuple1 = new double[][] { doubles1.ToArray() }.ToExamples();

            var doubles2 = TransitionDescriptor.Convert(stateP, true);
            var tuple2 = new double[][] { doubles2.ToArray() }.ToExamples();

            this.Learn(tuple1.Item1[0], tuple1.Item2[0], tuple2.Item1[0], tuple2.Item2[0]);
        }

        /// <summary>
        /// Reinforces the model from the new state, action and reward.
        /// </summary>
        /// <param name="x">Item features, i.e. the State.</param>
        /// <param name="y">Action label.</param>
        /// <param name="r">Reward value.</param>
        public abstract void Learn(Vector x, double y, double r);

        /// <summary>
        /// Reinforces the model from the new State, Action, StateP and Reward.
        /// </summary>
        /// <param name="x1">Item features, i.e. the State.</param>
        /// <param name="y">Action label.</param>
        /// <param name="x2">State/action reward value.</param>
        /// <param name="r">Reward value.</param>
        public abstract void Learn(Vector x1, double y, Vector x2, double r);
    }
}
