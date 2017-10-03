using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.AI.Functions;
using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised;
using numl.Utils;

namespace numl.Reinforcement.QLearning
{
    /// <summary>
    /// Q-Learner generator.
    /// </summary>
    public class QLearnerGenerator : ReinforcementGenerator 
    {
        /// <summary>
        /// Gets or sets the learning rate (alpha).
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the gamma (discount factor) value. A higher value will prefer long-term rewards over short-term ones.
        /// <para>This value should be between 0 and 1.</para>
        /// </summary>
        public double Gamma { get; set; }

        /// <summary>
        /// Gets or sets the Q-value initialization parameter for the Q-utility table.
        /// <para>A starting value needs to allow learning in uncertain environments - i.e. factoring in negative and positive rewards.  The default value is: -0.03.</para>
        /// </summary>
        public double QValue { get; set; }

        /// <summary>
        /// Gets or sets the Epsilon convergence parameter used in training.
        /// <para>This is the amount of change required before learning is considered converged. The default is: 0.0001.</para>
        /// </summary>
        public double Epsilon { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations for learning until convergence.
        /// </summary>
        public double MaxIterations { get; set; }

        /// <summary>
        /// Initializes a new QLearnerGenerator object.
        /// </summary>
        public QLearnerGenerator()
        {
            this.LearningRate = 0.1;
            this.Gamma = 0.3;
            this.QValue = -0.03;
            this.Epsilon = 10e-6;
            this.MaxIterations = 100;
        }

        /// <summary>
        /// Generates a <see cref="QLearnerModel"/> based on states/actions with transitions and rewards.
        /// </summary>
        /// <param name="X1">Initial State matrix.</param>
        /// <param name="y">Action label vector.</param>
        /// <param name="X2">Transition State matrix.</param>
        /// <param name="r">Reward values.</param>
        /// <returns>QLearnerModel.</returns>
        public override IReinforcementModel Generate(Matrix X1, Vector y, Matrix X2, Vector r)
        {
            this.Preprocess(X1, y, X2, r);

            var examples = MDPConverter.GetStates(X1, y, X2, this.FeatureProperties, this.FeatureDiscretizer);

            var states = examples.Item1; var actions = examples.Item2; var statesP = examples.Item3;

            QTable Q = new QTable();

            // construct Q table
            for (int i = 0; i < states.Count(); i++)
            {
                var state = states.ElementAt(i);
                var action = actions.ElementAt(i);
                var stateP = statesP.ElementAt(i);

                Q.AddOrUpdate(state, action, r[i]);

                if (!Q.ContainsKey(stateP))
                    Q.AddKey(stateP);
            }

            double count = states.Select(s => s.Id).Distinct().Count();

            double change = 0;
            for (int pass = 0; pass < this.MaxIterations; pass++)
            {
                change = 0;

                for (int i = 0; i < states.Count(); i++)
                {
                    IState state = states.ElementAt(i);
                    IAction action = actions.ElementAt(i);
                    IState stateP = statesP.ElementAt(i);
                    double reward = r[i];

                    double q = QLearnerModel.ComputeQ(Q, this.Gamma, this.LearningRate, state, action, stateP, reward);

                    change += (1.0 / count) * System.Math.Abs((Q[state, action] - q));

                    Q[state, action] = q;
                }

                if (change <= this.Epsilon)
                    break;
            }

            return new QLearnerModel()
            {
                Descriptor = this.Descriptor,
                TransitionDescriptor = this.TransitionDescriptor,
                NormalizeFeatures = this.NormalizeFeatures,
                FeatureNormalizer = this.FeatureNormalizer,
                FeatureProperties = this.FeatureProperties,
                FeatureDiscretizer = this.FeatureDiscretizer,
                LearningRate = this.LearningRate,
                Gamma = this.Gamma,
                Q = Q
            };
        }
    }
}
