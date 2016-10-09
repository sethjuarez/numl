// file:	Supervised\NeuralNetwork\Network.cs
//
// summary:	Implements the network class
using System;
using System.Linq;
using System.Collections.Generic;

using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Data;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A network.</summary>
    public partial class Network : Data.Graph
    {
        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        public Neuron[] In { get; set; } //TODO: Sync with graph

        /// <summary>Gets or sets the out.</summary>
        /// <value>The out.</value>
        public Neuron[] Out { get; set; } //TODO: Sync with graph

        /// <summary>
        /// Gets or sets the current loss of the network.
        /// </summary>
        public double Cost { get; set; } = 0d;

        /// <summary>Gets a label.</summary>
        /// <param name="n">The Node to process.</param>
        /// <param name="d">The Descriptor to process.</param>
        /// <returns>The label.</returns>
        internal static string GetLabel(int n, Descriptor d)
        {
            var label = "";
            try { label = Enum.GetName(d.Label.Type, n); }
            // TODO: fix cheesy way of getting around IsEnum
            catch
            {
                if (d.Label is StringProperty && ((StringProperty)d.Label).AsEnum)
                    label = ((StringProperty)d.Label).Dictionary[n];
                else label = d.Label.Name;
            }

            return label;
        }

        #region Computation

        /// <summary>Forwards the given x coordinate.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Vector to process.</param>
        public void Forward(Vector x)
        {
            if (In.Length != x.Length + 1)
                throw new InvalidOperationException("Input nodes not aligned to input vector");

            // this should forward the input through the network
            // from the input nodes through the layers to the output layer

            // set input (with bias inputs)
            for (int i = 0; i < In.Length; i++)
                In[i].Input = (i == 0 ? 1.0 : x[i - 1]);
            // evaluate
            for (int i = 0; i < Out.Length; i++)
                Out[i].Evaluate();
        }

        /// <summary>Backpropagates the errors through the network given the supplied label.</summary>
        /// <param name="y">Label to process.</param>
        /// <param name="properties">Network training properties for use in learning.</param>
        public void Back(double y, NetworkTrainingProperties properties)
        {
            this.Cost = Score.ComputeRMSE(Vector.Create(this.Out.Length, () => y), this.Out.Select(s => s.Output).ToVector());

            // propagate error gradients
            for (int i = 0; i < Out.Length; i++)
                Out[i].Error(y);

            // reset weights
            for (int i = 0; i < Out.Length; i++)
                Out[i].Update(properties);
        }

        /// <summary>Backpropagates the errors through the network given the supplied sequence label.</summary>
        /// <param name="y">Output vector to process.</param>
        /// <param name="properties">Network training properties for use in learning.</param>
        /// <param name="update">Indicates whether to update the weights after computing the errors.</param>
        public void Back(Vector y, NetworkTrainingProperties properties, bool update = true)
        {
            this.Cost = Score.ComputeRMSE(y, this.Out.Select(s => s.Output).ToVector());

            // CK
            // propagate error gradients
            for (int i = 0; i < Out.Length; i++)
                Out[i].Error(y[i]);

            if (update)
            {
                // reset weights
                for (int i = 0; i < Out.Length; i++)
                    Out[i].Update(properties);
            }
        }

        #endregion

        #region Graph

        /// <summary>
        /// Adds the Neuron to the underlying graph.
        /// </summary>
        /// <param name="node">Neuron to add.</param>
        /// <returns></returns>
        public Neuron AddVertex(Neuron node)
        {
            base.AddVertex(node);

            return node;
        }

        /// <summary>
        /// Adds the Edge to the underlying graph.
        /// </summary>
        /// <param name="edge">Edge to add.</param>
        /// <returns></returns>
        public Edge AddEdge(Edge edge)
        {
            base.AddEdge(edge);

            return edge;
        }

        #endregion

        /// <summary>
        /// Returns a new Network instance.
        /// </summary>
        public static Network New
        {
            get { return new Network(); }
        }

        /// <summary>
        /// Returns the Nodes from the specified layer, where 0 is the Input layer.
        /// </summary>
        /// <param name="layer">The layer index to retrieve Nodes from.</param>
        /// <returns></returns>
        public IEnumerable<Neuron> GetNodes(int layer)
        {
            return GetVertices().Where(n => ((Neuron)n).LayerId == layer)
                                .Select(n => (Neuron)n);
        }
    }
}
