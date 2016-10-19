using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// A Gated Recurrent Unit neural network generator.
    /// </summary>
    public class GatedRecurrentGenerator : Generator, ISequenceGenerator
    {
        /// <summary>Gets or sets the learning rate.</summary>
        /// <value>The learning rate.</value>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the Lambda or weight decay term.
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>Gets or sets the maximum iterations.</summary>
        /// <value>The maximum iterations.</value>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the size of the memory timeframe.
        /// <para>A larger value will allow the network to learn greater long-term dependencies.  This value should be less than the size of the training set.</para>
        /// </summary>
        public int SequenceLength { get; set; }

        /// <summary>
        /// Gets or sets the Reset gating function used in the individual neurons.
        /// </summary>
        public IFunction ResetGate { get; set; }

        /// <summary>
        /// Gets or sets the Update gating function used in the individual neurons.
        /// </summary>
        public IFunction UpdateGate { get; set; }

        /// <summary>Gets or sets the activation.</summary>
        /// <value>The activation.</value>
        public IFunction Activation { get; set; }

        /// <summary>
        /// Gets or sets the output layer function (i.e. Softmax).
        /// </summary>
        public IFunction OutputFunction { get; set; }

        /// <summary>
        /// Gets or sets the weight initialization value.
        /// <para>Weights will be randomly initialized between the range: -Epsilon and +Epsilon</para>
        /// </summary>
        public double Epsilon { get; set; }

        /// <summary>
        /// Initializes a new instance of a Gated Recurrent Network generator.
        /// </summary>
        public GatedRecurrentGenerator()
        {
            this.SequenceLength = 1;
            this.Epsilon = double.NaN;
            this.LearningRate = 0.1;
            this.Lambda = 0.0;
            this.ResetGate = new SteepLogistic();
            this.UpdateGate = new SteepLogistic();
            this.Activation = new Tanh();
            this.PreserveOrder = true;
        }

        /// <summary>
        /// Generates a GRU neural network from the training set.
        /// </summary>
        /// <param name="X">The Matrix of example data.</param>
        /// <param name="y">The vector of example labels.</param>
        /// <returns></returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            return this.Generate(X, y.ToMatrix(VectorType.Col));
        }

        /// <summary>
        /// Generates a GRU neural network model for predicting sequences.
        /// </summary>
        /// <param name="X">Matrix of training data.</param>
        /// <param name="Y">Matrix of matching sequence labels.</param>
        /// <returns>GatedRecurrentModel.</returns>
        public ISequenceModel Generate(Matrix X, Matrix Y)
        {
            this.Preprocess(X);

            // because Seth said so...
            if (MaxIterations <= 0)
                MaxIterations = 500;

            Network network = Network.New().Create(X.Cols, Y.Cols, Activation, OutputFunction,
                fnNodeInitializer: (i, j) => new RecurrentNeuron()
                {
                    ActivationFunction = this.Activation,
                    ResetGate = this.ResetGate,
                    MemoryGate = this.UpdateGate,

                    DeltaH = Vector.Zeros(this.SequenceLength)
                }, epsilon: Epsilon);

            var model = new GatedRecurrentModel
            {
                Descriptor = Descriptor,
                NormalizeFeatures = base.NormalizeFeatures,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Network = network,
                OutputFunction = this.OutputFunction
            };

            int m = X.Rows;

            OnModelChanged(this, ModelEventArgs.Make(model, "Initialized"));

            NetworkTrainingProperties properties = NetworkTrainingProperties.Create(network, X.Rows, X.Cols, this.LearningRate, this.Lambda, this.MaxIterations,
                                                    new { this.SequenceLength });

            Vector loss = Vector.Zeros(MaxIterations);

            var tuples = X.GetRows().Select((s, si) => new Tuple<Vector, Vector>(s, Y[si]));

            for (int pass = 0; pass < MaxIterations; pass++)
            {
                properties.Iteration = pass;

                tuples.Batch(SequenceLength, (idx, items) =>
                {
                    network.ResetStates(properties);

                    for (int i = 0; idx < items.Count(); idx++)
                    {
                        network.Forward(items.ElementAt(i).Item1);
                        network.Back(items.ElementAt(i).Item2, properties);
                    }

                }, asParallel: false);

                loss[pass] = network.Cost;

                var output = String.Format("Run ({0}/{1}): {2}", pass, MaxIterations, network.Cost);
                OnModelChanged(this, ModelEventArgs.Make(model, output));
            }

            return model;
        }
    }
}
