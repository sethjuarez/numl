// file:	Supervised\NeuralNetwork\Edge.cs
//
// summary:	Implements the edge class
using System;
using System.Linq;
using numl.Math.Probability;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>An edge.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Edge
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
        /// <summary>Gets or sets the source for the.</summary>
        /// <value>The source.</value>
        public Node Source { get; set; }
        /// <summary>Gets or sets the identifier of the source.</summary>
        /// <value>The identifier of the source.</value>
        [JsonProperty]
        internal string SourceId { get; set; }
        /// <summary>Gets or sets the Target for the.</summary>
        /// <value>The target.</value>
        public Node Target { get; set; }
        /// <summary>Gets or sets the identifier of the target.</summary>
        /// <value>The identifier of the target.</value>
        [JsonProperty]
        internal string TargetId { get; set; }
        /// <summary>Gets or sets the weight.</summary>
        /// <value>The weight.</value>
        [JsonProperty]
        public double Weight { get; set; }
        /// <summary>Creates a new Edge.</summary>
        /// <param name="source">Source for the.</param>
        /// <param name="target">Target for the.</param>
        /// <param name="weight">Weight</param>
        /// <returns>An Edge.</returns>
        public static Edge Create(Node source, Node target, double weight = 0)
        {
            Edge e = new Edge { Source = source, Target = target };
            e.SourceId = source.Id;
            e.TargetId = target.Id;
            source.Out.Add(e);
            target.In.Add(e);
            e.Weight = weight;
            return e;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() =>
            $"{Source} ---- {Weight} ----> {Target}";

    }
}
