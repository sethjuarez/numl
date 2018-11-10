using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.Model;
using numl.Math.LinearAlgebra;
using numl.Supervised;
using numl.Utils;

namespace numl.Reinforcement
{
    /// <summary>
    /// ReinforcementGenerator base class.
    /// </summary>
    public abstract class ReinforcementGenerator : IReinforcementGenerator
    {
        /// <summary>
        /// Event queue for all listeners interested in ModelChanged events.
        /// </summary>
        public event EventHandler<ModelEventArgs> ModelChanged;

        /// <summary>
        /// Raises the model event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information to send to registered event handlers.</param>
        protected virtual void OnModelChanged(object sender, ModelEventArgs e)
        {
            ModelChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Gets or sets the state/action descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }

        /// <summary>
        /// Gets or sets the transition/reward descriptor.
        /// </summary>
        public Descriptor TransitionDescriptor { get; set; }

        /// <summary>
        /// Gets or sets whether to perform feature normalisation using the specified Feature Normalizer.
        /// </summary>
        public bool NormalizeFeatures { get; set; }

        /// <summary>
        /// Gets or sets the feature normalizer to use for each item.
        /// </summary>
        public numl.Math.Normalization.INormalizer FeatureNormalizer { get; set; }

        /// <summary>
        /// Gets or sets the feature discretizer to use for reducing each item.
        /// </summary>
        public numl.Math.Discretization.IDiscretizer FeatureDiscretizer { get; set; }

        /// <summary>
        /// Gets or sets the Feature properties from the original training set.
        /// </summary>
        public numl.Math.Summary FeatureProperties { get; set; }

        /// <summary>
        /// Initializes a new ReinforcementGenerator instance.
        /// </summary>
        public ReinforcementGenerator()
        {
            this.NormalizeFeatures = false;
            this.FeatureNormalizer = new numl.Math.Normalization.MinMaxNormalizer();
        }

        /// <summary>
        /// Override to perform custom pre-processing steps on the raw Matrix data.
        /// </summary>
        /// <param name="X1">Matrix of initial states.</param>
        /// <param name="y">Vector of action labels.</param>
        /// <param name="X2">Matrix of transition states.</param>
        /// <param name="r">Vector of reward values.</param>
        /// <returns></returns>
        public virtual void Preprocess(Matrix X1, Vector y, Matrix X2, Vector r)
        {
            this.FeatureProperties = numl.Math.Summary.Summarize(X1);

            if (this.NormalizeFeatures)
            {
                if (this.FeatureNormalizer != null)
                {
                    for (int i = 0; i < X1.Rows; i++)
                    {
                        Vector v1 = this.FeatureNormalizer.Normalize(X1[i, VectorType.Row], this.FeatureProperties);
                        X1[i, VectorType.Row] = v1;

                        if (X2 != null)
                        {
                            Vector v2 = this.FeatureNormalizer.Normalize(X2[i, VectorType.Row], this.FeatureProperties);
                            X2[i, VectorType.Row] = v2;
                        }
                    }
                }
            }

            if (this.FeatureDiscretizer == null)
            {
                Matrix temp = Matrix.VStack(X1, X2);
                double[] bins = new double[temp.Cols];
                for (int x = 0; x < X1.Cols; x++)
                    bins[x] = temp[x, VectorType.Col].Distinct().Count();

                this.FeatureDiscretizer = new numl.Math.Discretization.BinningDiscretizer(bins.ToVector());
                this.FeatureDiscretizer.Initialize(X1, this.FeatureProperties);
            }
        }

        /// <summary>
        /// Generates a <see cref="IReinforcementModel"/> based on a set of temporal/continuous examples.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="description">The description.</param>
        /// <param name="examples">Example set.</param>
        /// <returns>IReinforcementModel.</returns>
        public IReinforcementModel Generate(Descriptor description, IEnumerable<object> examples)
        {
            if (!examples.Any()) throw new InvalidOperationException("Empty example set.");

            Descriptor = description;
            if (Descriptor.Features == null || Descriptor.Features.Length == 0)
                throw new InvalidOperationException("Invalid descriptor: Empty feature set!");
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Invalid descriptor: Empty label!");

            var doubles = Descriptor.Convert(examples);
            var (states, actions) = doubles.ToExamples();

            Vector rewards = Vector.Rand(actions.Length);

            var rewardProp = description.Features.GetPropertyOfType<RewardAttribute>();
            if (rewardProp != null)
            {
                for (int x = 0; x < examples.Count(); x++)
                {
                    rewards[x] = rewardProp.Convert(examples.ElementAt(x)).First();
                }
            }

            return this.Generate(states, actions, rewards);
        }

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> based on a set of temporal/continuous examples. 
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="descriptor">The description .</param>
        /// <param name="examples">Example set..</param>
        /// <returns></returns>
        public IReinforcementModel Generate<T>(Descriptor descriptor, IEnumerable<T> examples) where T : class
        {
            return Generate(descriptor, examples as IEnumerable<object>);
        }

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> based on a set of examples and transitions.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples1">Example training set.</param>
        /// <param name="examples2">Corresponding training set where each item represents a transition from <paramref name="examples1"/>.</param>
        /// <returns>IReinforcementModel.</returns>
        public IReinforcementModel Generate(IEnumerable<object> examples1, IEnumerable<object> examples2)
        {
            if (!examples1.Any()) throw new InvalidOperationException("Empty example set.");

            if (this.Descriptor == null)
                throw new InvalidOperationException("Descriptor is null");

            return Generate(this.Descriptor, examples1, this.TransitionDescriptor, examples2);
        }

        /// <summary>
        /// Generate an <see cref="IReinforcementModel"/> based on a set of examples and transitions.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="descriptor">The description.</param>
        /// <param name="examples1">Example training set or state/action pairs.</param>
        /// <param name="transitionDescriptor">(Optional) Descriptor for extracting transition state/reward information from <paramref name="examples2"/> set.</param>
        /// <param name="examples2">(Optional) Corresponding training set where each item represents a transition state from <paramref name="examples1"/> and a reward label.</param>
        /// <returns>IReinforcementModel.</returns>
        public IReinforcementModel Generate(Descriptor descriptor, IEnumerable<object> examples1, Descriptor transitionDescriptor, IEnumerable<object> examples2)
        {
            if (!examples1.Any()) throw new InvalidOperationException("Empty example set.");

            bool hasTransitionStates = (examples2 != null && examples2.Any());

            this.Descriptor = descriptor;
            this.TransitionDescriptor = transitionDescriptor;

            if (Descriptor.Features == null || Descriptor.Features.Length == 0)
                throw new InvalidOperationException("Invalid State Descriptor: Empty feature set!");
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Invalid State Descriptor: Empty label!");

            if (hasTransitionStates)
            {
                if (TransitionDescriptor == null || TransitionDescriptor.Features == null || TransitionDescriptor.Features.Length == 0)
                    throw new ArgumentNullException($"Transition Descriptor was null. A transition desciptor is required for '{nameof(examples2)}'.");
                if (examples2.Count() != examples1.Count())
                    throw new InvalidOperationException($"Length of '{nameof(examples1)}' must match length of '{nameof(examples2)}'.");
            }

            var doubles = this.Descriptor.Convert(examples1);
            var (states, actions) = doubles.ToExamples();

            Matrix statesP = null;
            Vector rewards = Vector.Rand(actions.Length);

            if (hasTransitionStates)
            {
                var doubles2 = this.TransitionDescriptor.Convert(examples2);
                (statesP, rewards) = doubles2.ToExamples();
            }
            else
            {
                statesP = new Matrix(states.Rows, states.Cols);
                // assume temporal
                for (int i = 0; i < states.Rows - 1; i++)
                {
                    statesP[i, VectorType.Row] = states[i + 1, VectorType.Row];
                }
            }

            return Generate(states, actions, statesP, rewards);
        }

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> from a descriptor and examples.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="descriptor">The description.</param>
        /// <param name="examples1">Example training set.</param>
        /// <param name="examples2">(Optional) Corresponding training set where each item represents a transition from <paramref name="examples1"/>.</param>
        /// <returns>IReinforcementModel</returns>
        public IReinforcementModel Generate<T>(Descriptor descriptor, IEnumerable<T> examples1, IEnumerable<T> examples2) where T : class
        {
            return Generate(descriptor, examples1 as IEnumerable<object>, examples2 as IEnumerable<object>);
        }

        /// <summary>
        /// Generates a <see cref="IReinforcementModel"/> from the state/action and corresponding reward training data.
        /// <para>This assumes a temporal sequence of training data where each row is continuous with the next.</para>
        /// </summary>
        /// <param name="X">Matrix of states or training example features.</param>
        /// <param name="y">Corresponding vector of actions.</param>
        /// <param name="r">Reward values for each state/action pair.</param>
        /// <returns>IReinforcementModel object.</returns>
        public virtual IReinforcementModel Generate(Matrix X, Vector y, Vector r)
        {
            // generate temporal data
            Matrix X2 = new Matrix(X.Rows, X.Cols);

            for (int i = 0; i < X.Rows - 1; i++)
            {
                X2[i, VectorType.Row] = X[i + 1, VectorType.Row];
            }

            return this.Generate(X, y, X2, r);
        }

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> from the state/action, transition states and reward training data.
        /// </summary>
        /// <param name="X1">Matrix of states or training example features.</param>
        /// <param name="y">Corresponding vector of actions.</param>
        /// <param name="X2">Matrix of corresponding transition states.</param>
        /// <param name="r">Reward values for each state/action pair.</param>
        /// <returns>IReinforcementModel object.</returns>
        public abstract IReinforcementModel Generate(Matrix X1, Vector y, Matrix X2, Vector r);
    }
}
