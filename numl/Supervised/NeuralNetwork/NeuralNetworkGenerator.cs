// file:	Supervised\NeuralNetwork\NeuralNetworkGenerator.cs
//
// summary:	Implements the neural network generator class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A neural network generator.</summary>
    public class NeuralNetworkGenerator : Generator
    {
        /// <summary>Gets or sets the learning rate.</summary>
        /// <value>The learning rate.</value>
        public double LearningRate { get; set; }
        /// <summary>Gets or sets the maximum iterations.</summary>
        /// <value>The maximum iterations.</value>
        public int MaxIterations { get; set; }
        /// <summary>Gets or sets the activation.</summary>
        /// <value>The activation.</value>
        public IFunction Activation { get; set; }

        /// <summary>Default constructor.</summary>
        public NeuralNetworkGenerator()
        {
            LearningRate = 0.9;
            MaxIterations = -1;
            Activation = new Tanh();
        }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            // because I said so...
            if (MaxIterations == -1) MaxIterations = x.Rows * 1000;

            var network = Network.Default(Descriptor, x, y, Activation);
            var model = new NeuralNetworkModel { Descriptor = Descriptor, Network = network };
            OnModelChanged(this, ModelEventArgs.Make(model, "Initialized"));

            for (int i = 0; i < MaxIterations; i++)
            {
                int idx = i % x.Rows;
                network.Forward(x[idx, VectorType.Row]);
                //OnModelChanged(this, ModelEventArgs.Make(model, "Forward"));
                network.Back(y[idx], LearningRate);
                var output = String.Format("Run ({0}/{1})", i, MaxIterations);
                OnModelChanged(this, ModelEventArgs.Make(model, output));
            }

            return model;
        }
    }
}
