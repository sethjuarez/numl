// file:	Supervised\NeuralNetwork\Neuron.cs
//
// summary:	Implements the neuron class
using System;
using System.Linq;
using numl.Math.Functions;
using System.Collections.Generic;
using numl.Utils;
using Newtonsoft.Json;

namespace numl.Supervised.NeuralNetwork
{
    
    /// <summary>A node.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Node 
    {
        private static int _id = -1;
        /// <summary>Default constructor.</summary>
        public Node()
        {
            // assume bias node unless
            // otherwise told through
            // links
            this.Constrained = false;
            Output = 1d;
            Input = 1d;
            Delta = 0d;
            Label = String.Empty;
            Out = new List<Edge>();
            In = new List<Edge>();
            Id = (++_id).ToString();
        }
        
        /// <summary>
        /// Gets or sets whether weights into this Node are constrained / preserved.
        /// </summary>
        public bool Constrained { get; set; }

        /// <summary>Gets or sets the output value.</summary>
        /// <value>The output.</value>
        public double Output { get; set; }

        /// <summary>Gets or sets the combined input value.</summary>
        /// <value>The input.</value>
        public double Input { get; set; }

        /// <summary>Gets or sets the delta.</summary>
        /// <value>The delta.</value>
        [JsonProperty]
        public double Delta { get; set; }

        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [JsonProperty]
        public string Label { get; set; }
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty]
        public string Id { get; private set; }
        /// <summary>Gets or sets the out.</summary>
        /// <value>The out.</value>
        public List<Edge> Out { get; set; }

        /// <summary>Gets or sets the Input <see cref="Node"/> connections.</summary>
        /// <value>The in.</value>
        public List<Edge> In { get; set; }

        /// <summary>Gets or sets the activation function.</summary>
        /// <value>The activation.</value>
		[JsonProperty]
        public IFunction ActivationFunction { get; set; }

        /// <summary>
        /// Gets or sets the output function (optional).
        /// </summary>
		[JsonProperty]
        public IFunction OutputFunction { get; set; }

        /// <summary>Calculates and returns the Node's <see cref="Output"/> value.</summary>
        /// <remarks>Input is set to the weights multiplied by the source <see cref="Node"/>'s Input.</remarks>
        /// <returns>A double.</returns>
        public virtual double Evaluate()
        {
            if (In.Count > 0)
            {
                Input = In.Select(e => e.Weight * e.Source.Evaluate()).Sum();
                Output = ActivationFunction.Compute(Input);
            }

            return Output;
        }

        /// <summary>Calculates and returns the error derivative (<see cref="Delta"/>) of this node.</summary>
        /// <param name="t">The double to process.</param>
        /// <returns>A double.</returns>
        public virtual double Error(double t)
        {
            // output node
            if (Out.Count == 0)
                Delta = Output - t;
            else // internal nodes
            {
                var hp = ActivationFunction.Derivative(Input);
                Delta = hp * Out.Select(e => e.Weight * e.Target.Error(t)).Sum();
            }

            return Delta;
        }
        /// <summary>Propogates a weight update event upstream through the network using the supplied learning rate.</summary>
        /// <param name="learningRate">The learning rate.</param>
        public virtual void Update(double learningRate)
        {
            foreach (Edge edge in In)
            {
                // update the weights on the input nodes
                // for output nodes, the derivative is the Delta
                if (!this.Constrained)
                {
                    edge.Weight = learningRate * Delta * edge.Source.Output;
                }
                edge.Source.Update(learningRate);
            }
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1} | {2})", Label, Input, Output);
        }
    }
}