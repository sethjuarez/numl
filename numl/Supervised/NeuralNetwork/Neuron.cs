// file:	Supervised\NeuralNetwork\Neuron.cs
//
// summary:	Implements the neuron class
using System;
using System.Linq;
using numl.Math.Functions;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A node.</summary>
    public class Node 
    {
        /// <summary>Default constructor.</summary>
        public Node()
        {
            // assume bias node unless
            // otherwise told through
            // links
            Output = 1d;
            Input = 1d;
            Delta = 0d;
            Label = String.Empty;
            Out = new List<Edge>();
            In = new List<Edge>();
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>Gets or sets the output.</summary>
        /// <value>The output.</value>
        public double Output { get; set; }
        /// <summary>Gets or sets the input.</summary>
        /// <value>The input.</value>
        public double Input { get; set; }
        /// <summary>Gets or sets the delta.</summary>
        /// <value>The delta.</value>
        public double Delta { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        public string Label { get; set; }
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }
        /// <summary>Gets or sets the out.</summary>
        /// <value>The out.</value>
        public List<Edge> Out { get; set; }
        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        public List<Edge> In { get; set; }
        /// <summary>Gets or sets the activation.</summary>
        /// <value>The activation.</value>
        public IFunction Activation { get; set; }
        /// <summary>Gets the evaluate.</summary>
        /// <returns>A double.</returns>
        public double Evaluate()
        {
            if (In.Count > 0)
            {
                Input = In.Select(e => e.Weight * e.Source.Evaluate()).Sum();
                Output = Activation.Compute(Input);
            }

            return Output;
        }
        /// <summary>Errors.</summary>
        /// <param name="t">The double to process.</param>
        /// <returns>A double.</returns>
        public double Error(double t)
        {
            // output node
            if (Out.Count == 0)
                Delta = Output - t;
            else // internal nodes
            {
                var hp = Activation.Derivative(Input);
                Delta = hp * Out.Select(e => e.Weight * e.Target.Error(t)).Sum();
            }

            return Delta;
        }
        /// <summary>Updates the given learningRate.</summary>
        /// <param name="learningRate">The learning rate.</param>
        public void Update(double learningRate)
        {
            foreach (Edge edge in In)
            {
                // for output nodes, the derivative is the Delta
                edge.Weight = learningRate * Delta * edge.Source.Output;
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