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
        private double _DeltaL = 0;

        /// <summary>Default constructor.</summary>
        public Node(bool isBias = false)
        {
            // assume bias node unless
            // otherwise told through
            // links
            this.Constrained = false;
            this.IsBias = isBias;
            Output = 1d;
            Input = 1d;
            Delta = 0d;
            Label = null;
            Out = new List<Edge>();
            In = new List<Edge>();
            Id = (++_id).ToString();
        }

        /// <summary>
        /// Gets or sets whether weights into this Node are constrained / preserved.
        /// </summary>
        [JsonProperty]
        public bool Constrained { get; set; }

        [JsonProperty]
        public bool IsBias { get; set; }

        /// <summary>Gets or sets the output value.</summary>
        /// <value>The output.</value>
        [JsonProperty]
        public double Output { get; set; }

        /// <summary>Gets or sets the combined input value.</summary>
        /// <value>The input.</value>
        [JsonProperty]
        public double Input { get; set; }

        /// <summary>Gets or sets the delta.</summary>
        /// <value>The delta.</value>
        [JsonProperty]
        public double Delta { get; set; }

        /// <summary>
        /// Gets or sets the minor delta term.
        /// </summary>
        public double delta { get; set; }

        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [JsonProperty]
        public string Label { get; set; }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty]
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the layer of this Node.
        /// </summary>
        [JsonProperty]
        public int LayerId { get; set; }

        [JsonProperty]
        public int NodeId { get; set; }

        /// <summary>Gets or sets the Output <see cref="Node"/> connections.</summary>
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
        /// <remarks>Input is equal to the weights multiplied by the source <see cref="Node"/>'s Output.</remarks>
        /// <returns>A double.</returns>
        public virtual double Evaluate()
        {
            if (In.Count > 0)
            {
                Input = In.Sum(e => e.Weight * e.Source.Evaluate());
            }

            Output = ActivationFunction.Compute(Input);

            if (OutputFunction != null)
                Output = OutputFunction.Compute(Input);

            return Output;
        }

        /// <summary>Calculates and returns the error derivative (<see cref="Delta"/>) of this node.</summary>
        /// <param name="t">The double to process.</param>
        /// <returns>A double.</returns>
        public virtual double Error(double t)
        {
            _DeltaL = Delta;

            if (Out.Count == 0)
                Delta = delta = -(t - Output);

            else
            {
                if (In.Count > 0 && Out.Count > 0)
                {
                    double hp = this.ActivationFunction.Derivative(this.Input);
                    delta = Out.Sum(e => e.Weight * t) * hp;
                }

                Delta = Out.Sum(s => s.Target.delta * this.Output);
            }

            if (this.In.Count > 0)
            {
                for (int edge = 0; edge < this.In.Count; edge++)
                {
                    this.In[edge].Source.Error(this.Delta);
                }
            }

            return Delta;
        }

        /// <summary>Propagates a weight update event upstream through the network using the supplied learning rate.</summary>
        /// <param name="properties">Network training properties.</param>
        public virtual void Update(NetworkTrainingProperties properties)
        {
            for (int edge = 0; edge < this.In.Count; edge++)
            {
                if (!this.Constrained)
                {
                    // using stochastic gradient descent averaged over training examples.
                    Delta = (1.0 / properties.Examples) * Delta;
                    this.In[edge].Weight = this.In[edge].Weight - properties.LearningRate * Delta;

                    // RMSProp (needs to move into a neural optimizer method)
                    //double mean = (properties.Epsilon * _DeltaL) + ((1.0 - properties.Epsilon) * (Delta * Delta));
                    //this.In[edge].Weight = this.In[edge].Weight - properties.LearningRate * (Delta / System.Math.Sqrt(mean * mean));
                }
                this.In[edge].Source.Update(properties);
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