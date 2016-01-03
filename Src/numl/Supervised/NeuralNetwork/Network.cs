// file:	Supervised\NeuralNetwork\Network.cs
//
// summary:	Implements the network class
using System;
using numl.Model;
using System.Linq;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A network.</summary>
    public class Network
    {
        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        public Node[] In { get; set; }

        /// <summary>Gets or sets the out.</summary>
        /// <value>The out.</value>
        public Node[] Out { get; set; }

        public double Cost { get; set; } = 0d;

        /// <summary>Defaults.</summary>
        /// <param name="d">The Descriptor to process.</param>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="activationFunction">The activation.</param>
        /// <param name="outputFunction">The ouput function for hidden nodes (Optional).</param>
        /// <returns>A Network.</returns>
        public static Network Default(Descriptor d, Matrix x, Vector y, IFunction activationFunction, IFunction outputFunction = null, double epsilon = double.NaN)
        {
            Network nn = new Network();

            // set output to number of choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;
            // identity funciton for bias nodes
            IFunction ident = new Ident();

            // set number of hidden units to (Input + Hidden) * 2/3 as basic best guess. 
            int hidden = (int)System.Math.Ceiling((double)(x.Cols + output) * 2.0 / 3.0);

            // creating input nodes
            nn.In = new Node[x.Cols + 1];
            nn.In[0] = new Node(true) { Label = "B0", ActivationFunction = ident };
            for (int i = 1; i < x.Cols + 1; i++)
                nn.In[i] = new Node { Label = d.ColumnAt(i - 1), ActivationFunction = ident };

            // creating hidden nodes
            Node[] h = new Node[hidden + 1];
            h[0] = new Node(true) { Label = "B1", ActivationFunction = ident };
            for (int i = 1; i < hidden + 1; i++)
                h[i] = new Node { Label = String.Format("H{0}", i), ActivationFunction = activationFunction, OutputFunction = outputFunction };

            // creating output nodes
            nn.Out = new Node[output];
            for (int i = 0; i < output; i++)
                nn.Out[i] = new Node { Label = GetLabel(i, d), ActivationFunction = activationFunction, OutputFunction = outputFunction };

            // link input to hidden. Note: there are
            // no inputs to the hidden bias node
            for (int i = 1; i < h.Length; i++)
                for (int j = 0; j < nn.In.Length; j++)
                    Edge.Create(nn.In[j], h[i], epsilon: epsilon);

            // link from hidden to output (full)
            for (int i = 0; i < nn.Out.Length; i++)
                for (int j = 0; j < h.Length; j++)
                    Edge.Create(h[j], nn.Out[i], epsilon: epsilon);

            return nn;
        }

        /// <summary>
        /// Creates a new fully connected deep neural network based on the supplied size and depth parameters.
        /// </summary>
        /// <param name="inputLayer">Neurons in the input layer.</param>
        /// <param name="outputLayer">Neurons in the output layer.</param>
        /// <param name="activationFunction">Activation function for the hidden and output layers.</param>
        /// <param name="outputFunction">(Optional) Output function of the the Nodes in the output layer (overrides the Activation function).</param>
        /// <param name="fnNodeInitializer">(Optional) Function to call for initializing new Nodes - supplying parameters for the layer and node index.</param>
        /// <param name="fnWeightInitializer">(Optional) Function to call for initializing the weights of each connection (including bias nodes).
        /// <para>Where int1 = Source layer (0 is input layer), int2 = Source Node, int3 = Target node in the next layer.</para></param>
        /// <param name="epsilon">Weight initialization parameter for random weight selection.  Weight will be in the range of: -epsilon to +epsilon.</param>
        /// <param name="hiddenLayers">An array of hidden neuron dimensions, where each element is the size of each layer (excluding bias nodes).</param>
        /// <returns>Returns an untrained neural network model.</returns>
        public static Network Create(int inputLayer, int outputLayer, IFunction activationFunction, IFunction outputFunction = null, Func<int, int, Node> fnNodeInitializer = null, 
            Func<int, int, int, double> fnWeightInitializer = null, double epsilon = double.NaN, params int[] hiddenLayers)
        {
            Network network = new Network();

            IFunction ident = new Ident();

            if (fnNodeInitializer == null)
                fnNodeInitializer = new Func<int, int, Node>((i, j) => new Node());

            if (fnWeightInitializer == null)
                fnWeightInitializer = new Func<int, int, int, double>((l, i, j) => double.NaN);

            // creating input nodes
            network.In = new Node[inputLayer + 1];
            network.In[0] = new Node(true) { Label = "B0", ActivationFunction = ident, LayerId = 0 };
            network.In[0].LayerId = network.In[0].NodeId = 0;

            for (int i = 1; i < inputLayer + 1; i++)
            {
                network.In[i] = fnNodeInitializer(0, i);
                network.In[i].Label = (network.In[i].Label ?? string.Format("I{0}", i));
                network.In[i].ActivationFunction = (network.In[i].ActivationFunction ?? ident);
                network.In[i].LayerId = 0;
                network.In[i].NodeId = i;
            }

            Node[] last = null;
            for (int layerIdx = 0; layerIdx < hiddenLayers.Length; layerIdx++)
            {
                // creating hidden nodes
                Node[] layer = new Node[hiddenLayers[layerIdx] + 1];
                layer[0] = new Node(true) { Label = "B1", ActivationFunction = ident, LayerId = layerIdx + 1 };
                layer[0].NodeId = 0;
                for (int i = 1; i < layer.Length; i++)
                {
                    layer[i] = fnNodeInitializer(layerIdx + 1, i);
                    layer[i].Label = (layer[i].Label ?? String.Format("H{0}.{1}", layerIdx + 1, i));
                    layer[i].ActivationFunction = (layer[i].ActivationFunction ?? activationFunction);
                    layer[i].OutputFunction = layer[i].OutputFunction;
                    layer[i].LayerId = layerIdx + 1;
                    layer[i].NodeId = i;
                }

                if (layerIdx > 0 && layerIdx < hiddenLayers.Length)
                {
                    // create hidden to hidden (full)
                    for (int i = 0; i < last.Length; i++)
                        for (int x = 1; x < layer.Length; x++)
                            Edge.Create(last[i], layer[x], weight: fnWeightInitializer(layerIdx, i, x), epsilon: epsilon);
                }
                else if (layerIdx == 0)
                {
                    // create input to hidden (full)
                    for (int i = 0; i < network.In.Length; i++)
                        for (int j = 1; j < layer.Length; j++)
                            Edge.Create(network.In[i], layer[j], weight: fnWeightInitializer(layerIdx, i, j), epsilon: epsilon);
                }

                last = layer;
            }

            // creating output nodes
            network.Out = new Node[outputLayer];
            for (int i = 0; i < outputLayer; i++)
            {
                network.Out[i] = fnNodeInitializer(hiddenLayers.Length + 1, i);
                network.Out[i].Label = (network.Out[i].Label ?? String.Format("O{0}", i));
                network.Out[i].ActivationFunction = (network.Out[i].ActivationFunction ?? activationFunction);
                network.Out[i].OutputFunction = (network.Out[i].OutputFunction ?? outputFunction);
                network.Out[i].LayerId = hiddenLayers.Length + 1;
                network.Out[i].NodeId = i;
            }

            // link from (last) hidden to output (full)
            for (int i = 0; i < network.Out.Length; i++)
                for (int j = 0; j < last.Length; j++)
                    Edge.Create(last[j], network.Out[i], weight: fnWeightInitializer(hiddenLayers.Length, j, i), epsilon: epsilon);

            return network;
        }

        /// <summary>
        /// Creates a new deep neural network based on the supplied inputs and layers.
        /// </summary>
        /// <param name="d">Descriptor object.</param>
        /// <param name="X">Training examples</param>
        /// <param name="y">Training labels</param>
        /// <param name="activationFunction">Activation Function for each output layer.</param>
        /// <param name="outputFunction">Ouput Function for each output layer.</param>
        /// <param name="hiddenLayers">The intermediary (hidden) layers / ensembles in the network.</param>
        /// <returns>A Deep Neural Network</returns>
        public static Network Create(Descriptor d, Matrix X, Vector y, IFunction activationFunction, IFunction outputFunction = null, params NetworkLayer[] hiddenLayers)
        {
            Network nn = new Network();
            // set output to number of choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;
            // identity function for bias nodes
            IFunction ident = new Ident();

            // creating input nodes
            nn.In = new Node[X.Cols + 1];
            nn.In[0] = new Node { Label = "B0", ActivationFunction = ident };
            for (int i = 1; i < X.Cols + 1; i++)
                nn.In[i] = new Node { Label = d.ColumnAt(i - 1), ActivationFunction = ident };

            // creating output nodes
            nn.Out = new Node[output];
            for (int i = 0; i < output; i++)
                nn.Out[i] = new Node { Label = GetLabel(i, d), ActivationFunction = activationFunction, OutputFunction = outputFunction };

            for (int layer = 0; layer < hiddenLayers.Count(); layer++)
            {
                if (layer == 0 && hiddenLayers[layer].IsAutoencoder)
                {
                    // init and train it.
                }

                // connect input with previous layer or input layer
                // connect last layer with output layer
            }

            // link input to hidden. Note: there are
            // no inputs to the hidden bias node
            //for (int i = 1; i < h.Length; i++)
            //    for (int j = 0; j < nn.In.Length; j++)
            //        Edge.Create(nn.In[j], h[i]);

            //// link from hidden to output (full)
            //for (int i = 0; i < nn.Out.Length; i++)
            //    for (int j = 0; j < h.Length; j++)
            //        Edge.Create(h[j], nn.Out[i]);

            return nn;
        }


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
        /// <param name="t">The double to process.</param>
        /// <param name="learningRate">The learning rate.</param>
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
        /// <param name="t">The double to process.</param>
        /// <param name="learningRate">The learning rate.</param>
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

        /// <summary>Propagates a Delta reset event through the network starting from the output Node.</summary>
        /// <param name="newDelta">New delta value to apply to each Node.</param>
        //public void Reset(double newDelta)
        //{
        //    for (int i = 0; i < Out.Length; i++)
        //        Out[i].Reset(newDelta);
        //}

        /// <summary>The nodes.</summary>
        private HashSet<string> _nodes;
        /// <summary>Gets the nodes in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the nodes in this collection.
        /// </returns>
        public IEnumerable<Node> GetNodes()
        {
            if (_nodes == null) _nodes = new HashSet<string>();
            else _nodes.Clear();

            foreach (var node in Out)
            {
                _nodes.Add(node.Label);
                yield return node;
                foreach (var n in GetNodes(node))
                {
                    if (!_nodes.Contains(n.Label))
                    {
                        _nodes.Add(n.Label);
                        yield return n;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Nodes from the specified layer, where 0 is the Input layer.
        /// </summary>
        /// <param name="layer">The layer index to retrieve Nodes from.</param>
        /// <returns></returns>
        public IEnumerable<Node> GetNodes(int layer)
        {
            return GetNodes().Where(n => n.LayerId == layer);
        }

        /// <summary>Gets all nodes leading into the supplied Node.</summary>
        /// <param name="n">The Node to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the nodes in this collection.
        /// </returns>
        private static IEnumerable<Node> GetNodes(Node n)
        {
            foreach (var edge in n.In)
            {
                yield return edge.Source;
                foreach (var node in GetNodes(edge.Source))
                    yield return node;
            }
        }

        /// <summary>The edges.</summary>
        private HashSet<Tuple<string, string>> _edges;

        /// <summary>Gets the edges in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the edges in this collection.
        /// </returns>
        public IEnumerable<Edge> GetEdges()
        {
            if (_edges == null) _edges = new HashSet<Tuple<string, string>>();
            else _edges.Clear();

            foreach (var node in Out)
            {
                foreach (var edge in GetEdges(node))
                {
                    var key = new Tuple<string, string>(edge.Source.Id, edge.Target.Id);
                    if (!_edges.Contains(key))
                    {
                        _edges.Add(key);
                        yield return edge;
                    }
                }
            }
        }

        /// <summary>Gets all edges leading into the supplied Node.</summary>
        /// <param name="n">The Node to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the edges in this collection.
        /// </returns>
        private static IEnumerable<Edge> GetEdges(Node n)
        {
            foreach (var edge in n.In)
            {
                yield return edge;
                foreach (var e in GetEdges(edge.Source))
                    yield return e;
            }
        }
    }
}
