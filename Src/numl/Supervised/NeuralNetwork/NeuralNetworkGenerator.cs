// file:	Supervised\NeuralNetwork\NeuralNetworkGenerator.cs
//
// summary:	Implements the neural network generator class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A neural network generator.</summary>
    public class NeuralNetworkGenerator : Generator, ISequenceGenerator
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

        /// <summary>Default constructor.</summary>
        public NeuralNetworkGenerator()
        {
            LearningRate = 0.1;
            MaxIterations = -1;
            Epsilon = double.NaN;
            Activation = new Tanh();
        }

        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            return this.Generate(X, y.ToMatrix(VectorType.Col));
        }

        public virtual ISequenceModel Generate(Matrix X, Matrix Y)
        {
            this.Preprocess(X);
            // because I said so...
            if (MaxIterations == -1) MaxIterations = 500;

            var network = Network.New().Create(X.Cols, Y.Cols, Activation, OutputFunction, epsilon: Epsilon);

            var model = new NeuralNetworkModel
            {
                Descriptor = Descriptor,
                NormalizeFeatures = base.NormalizeFeatures,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Network = network
            };

            OnModelChanged(this, ModelEventArgs.Make(model, "Initialized"));

            NetworkTrainingProperties properties = NetworkTrainingProperties.Create(network, X.Rows, X.Cols, this.LearningRate, this.Lambda, this.MaxIterations);

            for (int i = 0; i < MaxIterations; i++)
            {
                properties.Iteration = i;

                for (int x = 0; x < X.Rows; x++)
                {
                    network.Forward(X[x, VectorType.Row]);
                    //OnModelChanged(this, ModelEventArgs.Make(model, "Forward"));
                    network.Back(Y[x, VectorType.Row], properties);
                }

                var output = String.Format("Run ({0}/{1}): {2}", i, MaxIterations, network.Cost);
                OnModelChanged(this, ModelEventArgs.Make(model, output));
            }

            return model;
        }
    }
}
