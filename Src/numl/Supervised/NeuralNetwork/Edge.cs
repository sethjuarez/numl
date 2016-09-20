// file:	Supervised\NeuralNetwork\Edge.cs
//
// summary:	Implements the edge class
using System;
using System.Linq;
using numl.Math.Probability;
using System.Collections.Generic;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>An edge.</summary>
    public class Edge : Data.IEdge
    {
        /// <summary>Default constructor.</summary>
        public Edge()
        {
            // random initialization
            // R. D. Reed and R. J. Marks II, "Neural Smithing: 
            // Supervised Learning in Feedforward Artificial 
            // Neural Networks", Mit Press, 1999. pg 57
            // selecting values from range [-a,+a] where 0.1 < a < 2
            Weight = (double)Sampling.GetUniform(1, 20) / 10d;
            if (Sampling.GetUniform() < .5)
                Weight *= -1;
        }
        /// <summary>Gets or sets the source Node.</summary>
        /// <value>The source.</value>
        public Neuron Source { get; set; }
        /// <summary>Gets or sets the identifier of the source.</summary>
        /// <value>The identifier of the source.</value>
        public int ParentId { get; set; }
        /// <summary>Gets or sets the target Node.</summary>
        /// <value>The target.</value>
        public Neuron Target { get; set; }
        /// <summary>Gets or sets the identifier of the target.</summary>
        /// <value>The identifier of the target.</value>
        public int ChildId { get; set; }
        /// <summary>Gets or sets the weight.</summary>
        /// <value>The weight.</value>
        public double Weight { get; set; }
        /// <summary>Creates a new Edge.</summary>
        /// <param name="source">Source for the.</param>
        /// <param name="target">Target for the.</param>
        /// <param name="weight">Weight parameter to initialize with.</param>
        /// <param name="epsilon">Seed value to use when randomly selecting a weight (ignored when <paramref name="weight"/> is supplied).</param>
        /// <returns>An Edge.</returns>
        public static Edge Create(Neuron source, Neuron target, double weight = double.NaN, double epsilon = double.NaN)
        {
            Edge e = new Edge { Source = source, Target = target };
            e.ParentId = source.Id;
            e.ChildId = target.Id;
            source.Out.Add(e);
            target.In.Add(e);

            if (!double.IsNaN(weight))
                e.Weight = weight;
            else if (!double.IsNaN(epsilon))
                e.Weight = Sampling.GetUniform(-epsilon, epsilon);

            return e;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() =>
            $"{Source} ---- {Weight} ----> {Target}";

    }
}
