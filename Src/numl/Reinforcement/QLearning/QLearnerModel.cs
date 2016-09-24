using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.AI.Functions;
using numl.Math.Functions;
using numl.Math.Discretization;
using numl.Math.LinearAlgebra;
using numl.Reinforcement.Functions;

namespace numl.Reinforcement.QLearning
{
    /// <summary>
    /// A Q-Learner Model.
    /// </summary>
    public class QLearnerModel : ReinforcementModel
    {
        /// <summary>
        /// Gets or sets the learning rate (alpha).
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the lambda (discount factor) value. A higher value will prefer long-term rewards over immediate rewards.
        /// <para>This value should be between 0 and 1.</para>
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// Gets or sets the Q utility table.
        /// </summary>
        public QTable Q { get; set; }

        /// <summary>
        /// Initializes a new Q-Learner model.
        /// </summary>
        public QLearnerModel()
        {
            this.Q = new QTable();
        }

        /// <summary>
        /// Predicts the best action for the current state.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public override double Predict(Vector y)
        {
            double state = this.FeatureDiscretizer.Discretize(y, this.FeatureProperties);

            return Q.GetMaxAction((int) state);
        }

        /// <summary>
        /// Updates the Q-Learner model by reinforcing with the new state/action feedback values.
        /// </summary>
        /// <param name="x">State vector.</param>
        /// <param name="y">Action label.</param>
        /// <param name="r">Reward value.</param>
        public override void Learn(Vector x, double y, double r)
        {
            var state = this.Q.Keys.Last();
            var stateP = MDPConverter.GetState(x, this.FeatureProperties, this.FeatureDiscretizer);
            var action = MDPConverter.GetAction(y, state.Id, stateP.Id);

            this.Q.AddOrUpdate(stateP, action, r);

            this.Q[state, action] = (1.0 - this.LearningRate) * Q[state, action]
                                        + this.LearningRate * (r + this.Lambda * Q[stateP, Q.GetMaxAction(stateP)]);
        }

        /// <summary>
        /// Updates the Q-Learner model by reinforcing with the new state/action and transition state feedback values.
        /// </summary>
        /// <param name="x1">Item features, i.e. the original State.</param>
        /// <param name="y">Action label.</param>
        /// <param name="x2">Transition state value.</param>
        /// <param name="r">Reward value.</param>
        public override void Learn(Vector x1, double y, Vector x2, double r)
        {
            var state = MDPConverter.GetState(x1, this.FeatureProperties, this.FeatureDiscretizer);
            var stateP = MDPConverter.GetState(x2, this.FeatureProperties, this.FeatureDiscretizer);
            var action = MDPConverter.GetAction(y, state.Id, stateP.Id);

            if (!Q.ContainsKey(state))
            {
                Q.AddOrUpdate(state, action, r);
            }

            if (!Q.ContainsKey(stateP))
                Q.AddKey(stateP);

            this.Q[state, action] = (1.0 - this.LearningRate) * Q[state, action]
                                        + this.LearningRate * (r + this.Lambda * Q[stateP, Q.GetMaxAction(stateP)]);
        }
    }
}
