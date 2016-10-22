// file:	Supervised\NeuralNetwork\Network.cs
//
// summary:	Implements the network class
using System;
using numl.Model;
using System.Linq;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A network.</summary>
    public static class NetworkOps
    {
        /// <summary>Defaults.</summary>
        /// <param name="d">The Descriptor to process.</param>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="activationFunction">The activation.</param>
        /// <param name="outputFunction">The ouput function for hidden nodes (Optional).</param>
        /// <param name="epsilon">epsilon</param>
        /// <returns>A Network.</returns>
        public static Network Create(this Network network, Descriptor d, Matrix x, Vector y, IFunction activationFunction, IFunction outputFunction = null, double epsilon = double.NaN)
        {
            // set output to number of choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;
            // identity funciton for bias nodes
            IFunction ident = new Ident();

            // set number of hidden units to (Input + Hidden) * 2/3 as basic best guess. 
            int hidden = (int)System.Math.Ceiling((double)(x.Cols + output) * 2.0 / 3.0);

            return network.Create(x.Cols, output, activationFunction, outputFunction,
                fnNodeInitializer: new Func<int, int, Neuron>((l, i) =>
                {
                    if (l == 0) return new Neuron(false) { Label = d.ColumnAt(i - 1), ActivationFunction = activationFunction, NodeId = i, LayerId = l };
                    else if (l == 2) return new Neuron(false) { Label = Network.GetLabel(i, d), ActivationFunction = activationFunction, NodeId = i, LayerId = l };
                    else return new Neuron(false) { ActivationFunction = activationFunction, NodeId = i, LayerId = l };
                }), hiddenLayers: hidden);
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
        public static Network Create(this Network network, int inputLayer, int outputLayer, IFunction activationFunction, IFunction outputFunction = null, Func<int, int, Neuron> fnNodeInitializer = null, 
            Func<int, int, int, double> fnWeightInitializer = null, double epsilon = double.NaN, params int[] hiddenLayers)
        {
            IFunction ident = new Ident();

            if (hiddenLayers == null || hiddenLayers.Length == 0)
                hiddenLayers = new int[] { (int) System.Math.Ceiling((inputLayer + outputLayer + 1) * (2.0 / 3.0)) };

            List<double> layers = new List<double>();
            layers.Add(inputLayer);
            foreach (int l in hiddenLayers)
                layers.Add(l + 1);
            layers.Add(outputLayer);

            if (fnNodeInitializer == null)
                fnNodeInitializer = new Func<int, int, Neuron>((i, j) => new Neuron());

            if (fnWeightInitializer == null)
                fnWeightInitializer = new Func<int, int, int, double>((l, i, j) => {
                    double inputs = (l > 0 ? layers[l - 1] : 0);
                    double outputs = (l < layers.Count - 1 ? layers[l + 1] : 0);
                    double eps = (double.IsNaN(epsilon) ? Edge.GetEpsilon(activationFunction.Minimum, activationFunction.Maximum, inputs, outputs) : epsilon);
                    return Edge.GetWeight(eps);
                });

            // creating input nodes
            network.In = new Neuron[inputLayer + 1];
            network.In[0] = network.AddNode(new Neuron(true) { Label = "B0", ActivationFunction = ident, NodeId = 0, LayerId = 0 });

            for (int i = 1; i < inputLayer + 1; i++)
            {
                network.In[i] = fnNodeInitializer(0, i);
                network.In[i].Label = (network.In[i].Label ?? string.Format("I{0}", i));
                network.In[i].ActivationFunction = (network.In[i].ActivationFunction ?? ident);
                network.In[i].LayerId = 0;
                network.In[i].NodeId = i;

                network.AddNode(network.In[i]);
            }

            Neuron[] last = null;
            for (int layerIdx = 0; layerIdx < hiddenLayers.Length; layerIdx++)
            {
                // creating hidden nodes
                Neuron[] layer = new Neuron[hiddenLayers[layerIdx] + 1];
                layer[0] = network.AddNode(new Neuron(true) { Label = $"B{layerIdx + 1}", ActivationFunction = ident, LayerId = layerIdx + 1, NodeId = 0 });
                for (int i = 1; i < layer.Length; i++)
                {
                    layer[i] = fnNodeInitializer(layerIdx + 1, i);
                    layer[i].Label = (layer[i].Label ?? String.Format("H{0}.{1}", layerIdx + 1, i));
                    layer[i].ActivationFunction = (layer[i].ActivationFunction ?? activationFunction);
                    layer[i].OutputFunction = layer[i].OutputFunction;
                    layer[i].LayerId = layerIdx + 1;
                    layer[i].NodeId = i;

                    network.AddNode(layer[i]);
                }

                if (layerIdx > 0 && layerIdx < hiddenLayers.Length)
                {
                    // create hidden to hidden (full)
                    for (int i = 0; i < last.Length; i++)
                        for (int x = 1; x < layer.Length; x++)
                            network.AddEdge(Edge.Create(last[i], layer[x], weight: fnWeightInitializer(layerIdx, i, x), epsilon: epsilon));
                }
                else if (layerIdx == 0)
                {
                    // create input to hidden (full)
                    for (int i = 0; i < network.In.Length; i++)
                        for (int j = 1; j < layer.Length; j++)
                            network.AddEdge(Edge.Create(network.In[i], layer[j], weight: fnWeightInitializer(layerIdx, i, j), epsilon: epsilon));
                }

                last = layer;
            }

            // creating output nodes
            network.Out = new Neuron[outputLayer];
            for (int i = 0; i < outputLayer; i++)
            {
                network.Out[i] = fnNodeInitializer(hiddenLayers.Length + 1, i);
                network.Out[i].Label = (network.Out[i].Label ?? String.Format("O{0}", i));
                network.Out[i].ActivationFunction = (network.Out[i].ActivationFunction ?? activationFunction);
                network.Out[i].OutputFunction = (network.Out[i].OutputFunction ?? outputFunction);
                network.Out[i].LayerId = hiddenLayers.Length + 1;
                network.Out[i].NodeId = i;

                network.AddNode(network.Out[i]);
            }

            // link from (last) hidden to output (full)
            for (int i = 0; i < network.Out.Length; i++)
                for (int j = 0; j < last.Length; j++)
                    network.AddEdge(Edge.Create(last[j], network.Out[i], weight: fnWeightInitializer(hiddenLayers.Length, j, i), epsilon: epsilon));

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
        public static Network Create(this Network network, Descriptor d, Matrix X, Vector y, IFunction activationFunction, IFunction outputFunction = null, params NetworkLayer[] hiddenLayers)
        {
            // set output to number of choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;
            // identity function for bias nodes
            IFunction ident = new Ident();

            // creating input nodes
            network.In = new Neuron[X.Cols + 1];
            network.In[0] = new Neuron { Label = "B0", ActivationFunction = ident };
            for (int i = 1; i < X.Cols + 1; i++)
                network.In[i] = new Neuron { Label = d.ColumnAt(i - 1), ActivationFunction = ident };

            // creating output nodes
            network.Out = new Neuron[output];
            for (int i = 0; i < output; i++)
                network.Out[i] = new Neuron { Label = Network.GetLabel(i, d), ActivationFunction = activationFunction, OutputFunction = outputFunction };

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

            return network;
        }

        /// <summary>
        /// Adds new connections for the specified node for the parent and child nodes.
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="node">Neuron being added.</param>
        /// <param name="parentNodes">Parent nodes that this neuron is connected with.</param>
        /// <param name="childNodes">Child nodes that this neuron is connected to.</param>
        /// <param name="epsilon">Weight initialization parameter.</param>
        /// <returns></returns>
        public static Network AddConnections(this Network network, Neuron node, IEnumerable<Neuron> parentNodes, IEnumerable<Neuron> childNodes, double epsilon = double.NaN)
        {
            if (epsilon == double.NaN)
                epsilon = Edge.GetEpsilon(node.ActivationFunction.Minimum, node.ActivationFunction.Maximum, parentNodes.Count(), childNodes.Count());

            if (parentNodes != null)
            {
                for (int i = 0; i < parentNodes.Count(); i++)
                    network.AddEdge(Edge.Create(parentNodes.ElementAt(i), node, epsilon: epsilon));
            }

            if (childNodes != null)
            {
                for (int j = 0; j < childNodes.Count(); j++)
                    network.AddEdge(Edge.Create(node, childNodes.ElementAt(j), epsilon: epsilon));
            }

            return network;
        }

        /// <summary>
        /// Gets the weight values as an [i x j] weight matrix.  Where i represents the node in the next layer (layer + 1) and j represents the node in the specified <paramref name="layer"/>.
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="layer">The layer to retrieve weights for.  The layer should be between 0 and the last hidden layer.</param>
        /// <param name="includeBiases">Indicates whether bias weights are included in the output.</param>
        /// <returns>Matrix.</returns>
        public static Matrix GetWeights(this Network network, int layer = 0, bool includeBiases = false)
        {
            if (layer >= network.Layers - 1)
                throw new ArgumentOutOfRangeException(nameof(layer), $"There are no weights from the output layer [{layer}].");

            var nodes = network.GetNodes(layer + 1).ToArray();
            int pos = ((network.Layers - 1 == layer + 1) ? 0 : 1);
            int bpos = (includeBiases ? 0 : 1);

            Matrix weights = Matrix.Zeros(nodes.Length - pos, network.GetNodes(layer).Count() - bpos);

            for (int i = pos; i < nodes.Length; i++)
            {
                for (int j = bpos; j < nodes[i].In.Count; j++)
                {
                    if (!nodes[i].In[j].Source.IsBias || (nodes[i].In[j].Source.IsBias && includeBiases))
                    {
                        weights[i - pos, j - bpos] = nodes[i].In[j].Weight;
                    }
                }
            }

            return weights;
        }

        /// <summary>
        /// Gets a bias input vector for the specified layer.  Each item is the bias weight on the connecting node in the next layer.
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="layer">Forward layer of biases and their weights.  The layer should be between 0 (first hidden layer) and the last hidden layer.</param>
        /// <returns>Vector.</returns>
        public static Vector GetBiases(this Network network, int layer = 0)
        {
            if (layer > network.Layers - 1)
                throw new ArgumentOutOfRangeException(nameof(layer), $"There are no bias nodes from the output layer [{layer}].");

            var nodes = network.GetNodes(layer + 1).ToArray();

            Vector biases = Vector.Zeros(nodes.Where(w => !w.IsBias).Count());

            for (int i = 0; i < nodes.Length; i++)
            {
                if (!nodes[i].IsBias)
                {
                    var bias = nodes[i].In.FirstOrDefault(f => f.Source.IsBias);
                    biases[i] = bias.Weight;
                }
            }

            return biases;
        }

        /// <summary>
        /// Links a Network from nodes and edges.
        /// </summary>
        /// <param name="nodes">An array of nodes in the network</param>
        /// <param name="edges">An array of edges between the nodes in the network.</param>
        public static Network LinkNodes(this Network network, IEnumerable<Neuron> nodes, IEnumerable<Edge> edges)
        {
            int inputLayerId = nodes.Min(m => m.LayerId);
            int outputLayerId = nodes.Max(m => m.LayerId);

            network.In = nodes.Where(w => w.LayerId == inputLayerId).ToArray();

            foreach (var node in network.In)
                network.AddNode(node);

            int hiddenLayer = inputLayerId + 1;
            // relink nodes
            Neuron[] last = null;
            for (int layerIdx = hiddenLayer; layerIdx < outputLayerId; layerIdx++)
            {
                Neuron[] layer = nodes.Where(w => w.LayerId == layerIdx).ToArray();

                foreach (var node in layer)
                    network.AddNode(node);

                if (layerIdx > hiddenLayer)
                {
                    // create hidden to hidden (full)
                    for (int i = 0; i < last.Length; i++)
                        for (int x = 1; x < layer.Length; x++)
                            network.AddEdge(Edge.Create(last[i], layer[x], 
                                weight: edges.First(f => f.ParentId == last[i].Id && f.ChildId == layer[x].Id).Weight));
                }
                else if (layerIdx == hiddenLayer)
                {
                    // create input to hidden (full)
                    for (int i = 0; i < network.In.Length; i++)
                        for (int j = 1; j < layer.Length; j++)
                            network.AddEdge(Edge.Create(network.In[i], layer[j], 
                                weight: edges.First(f => f.ParentId == network.In[i].Id && f.ChildId == layer[j].Id).Weight));
                }

                last = layer;
            }

            network.Out = nodes.Where(w => w.LayerId == outputLayerId).ToArray();

            foreach (var node in network.Out)
                network.AddNode(node);

            for (int i = 0; i < network.Out.Length; i++)
                for (int j = 0; j < last.Length; j++)
                    network.AddEdge(Edge.Create(last[j], network.Out[i], 
                        weight: edges.First(f => f.ParentId == last[j].Id && f.ChildId == network.Out[i].Id).Weight));

            return network;
        }

        /// <summary>
        /// Constrains the weights in the specified layer from being updated.  This prevents weights in pretrained layers from being updated.
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="layer">The layer of weights to constrain.  To prevent all weights from being changed specify the global value of -1.</param>
        /// <param name="constrain">Sets the <see cref="Neuron.Constrained"/> value to true/false.</param>
        /// <returns></returns>
        public static Network Constrain(this Network network, int layer = -1, bool constrain = true)
        {
            var nodes = (layer >= 0 ? network.GetNodes(layer) : network.GetVertices());
            
            foreach (Neuron node in nodes)
            {
                node.Constrained = constrain;
            }

            return network;
        }

        /// <summary>
        /// Reindexes each node's layer and label in the network, starting from 0 (input layer).
        /// </summary>
        /// <param name="network">Network to reindex.</param>
        /// <returns></returns>
        public static Network Reindex(this Network network)
        {
            var nodes = network.GetVertices().OfType<Neuron>()
                                             .GroupBy(g => g.LayerId)
                                             .OrderBy(o => o.Key)
                                             .ToArray();

            int layer = 0;

            foreach (var group in nodes)
            {
                int count = 0;

                foreach (var node in group)
                {
                    node.LayerId = layer;

                    // update labels.
                    if (node.IsInput) node.Label = $"I{count}";
                    if (node.IsBias) node.Label = $"B{layer}";
                    if (node.IsHidden) node.Label = $"H{layer}.{count}";
                    if (node.IsOutput) node.Label = $"O{count}";

                    count++;
                }

                layer++;
            }

            return network;
        }

        /// <summary>
        /// Prunes the network in the given direction for the specified number of layers.
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="backwards">If true, removes layer by layer in reverse order (i.e. output layer first).</param>
        /// <param name="layers">Number of layers to prune from the network.</param>
        public static Network Prune(this Network network, bool backwards = true, int layers = 1)
        {
            int count = layers;
            int layer = (backwards ? (network.Layers - 1) : 0);

            while (count > 0)
            {
                count--;

                var nodes = network.GetNodes(layer);

                for (int i = 0; i < nodes.Count(); i++)
                    network.RemoveNode(nodes.ElementAt(i));

                layer = (backwards ? --layer : ++layer);
            }

            network.Reindex();

            // remove bias outputs
            var outputs = network.GetNodes(network.Layers - 1).Where(w => w.IsBias);
            for (int x = 0; x < outputs.Count(); x++)
                network.RemoveNode(outputs.ElementAt(x));

            for (int i = 0; i < network.GetNodes(0).Count(); i++)
                network.GetNodes(0).ElementAt(i).In.Clear();

            network.In = network.GetNodes(0).ToArray();

            for (int i = 0; i < network.GetNodes(network.Layers - 1).Count(); i++)
                network.GetNodes(network.Layers - 1).ElementAt(i).Out.Clear();

            network.Out = network.GetNodes(network.Layers - 1).ToArray();

            return network;
        }

        /// <summary>
        /// Stacks the given networks in order, on top of the current network, to create a fully connected deep neural network.
        /// <para>This is useful in building pretrained multi-layered neural networks, where each layer is partially trained prior to stacking.</para>
        /// </summary>
        /// <param name="network">Current network.</param>
        /// <param name="removeInputs">If true, the input nodes in additional layers are removed prior to stacking.
        ///     <para>This will link the previous network's output layer with the hidden units of the next layer.</para>
        /// </param>
        /// <param name="removeOutputs">If true, output nodes in the input and middle layers are removed prior to stacking.
        ///     <para>This will link the previous network's hidden or output layer with the input or hidden units (when <paramref name="removeInputs"/> is true) in the next layer.</para>
        /// </param>
        /// <param name="addBiases">If true, missing bias nodes are automatically added within new hidden layers.</param>
        /// <param name="constrain">If true, the weights within each network are constrained leaving the new interconnecting network weights for training.</param>
        /// <param name="networks">Network objects to stack on top of the current network.  Each network is added downstream from the input nodes.</param>
        public static Network Stack(this Network network, bool removeInputs = false, bool removeOutputs = false, bool addBiases = true, 
                                        bool constrain = true, params Network[] networks)
        {
            IFunction ident = new Ident();

            // prune output layer on first (if pruning)
            Network deep = (removeOutputs ? network.Prune(true, 1) : network);

            if (constrain) deep.Constrain();

            // get the current network's output layer
            List<Neuron> prevOutput = deep.Out.ToList();

            for (int x = 0; x < networks.Length; x++)
            {
                Network net = networks[x];

                if (constrain) net.Constrain();
                // remove input layer on next network (if pruning)
                if (removeInputs) net = net.Prune(false, 1);
                // remove output layer on next (middle) network (if pruning)
                if (removeOutputs && x < networks.Length - 1) net = net.Prune(true, 1);

                // add biases (for hidden network layers)
                if (addBiases)
                {
                    if (!prevOutput.Any(a => a.IsBias == true))
                    {
                        int layerId = prevOutput.First().LayerId;
                        var bias = new Neuron(true)
                        {
                            Label = $"B{layerId}",
                            ActivationFunction = ident,
                            NodeId = 0,
                            LayerId = layerId
                        };

                        // add to graph
                        deep.AddNode(bias);
                        // copy to previous network's output layer (for reference)
                        prevOutput.Insert(0, bias);
                    }
                }

                int deepLayers = deep.Layers;
                Neuron[] prevLayer = null;
                var layers = net.GetVertices().OfType<Neuron>()
                                              .GroupBy(g => g.LayerId)
                                              .ToArray();

                for (int layer = 0; layer < layers.Count(); layer++)
                {
                    // get nodes in current layer of current network
                    var nodes = layers.ElementAt(layer).ToArray();
                    // set new layer ID (relative to pos. in resulting graph)
                    int layerId = layer + deepLayers;

                    foreach (var node in nodes)
                    {
                        // set the new layer ID
                        node.LayerId = layerId;
                        // add to graph
                        deep.AddNode(node);

                        if (!node.IsBias)
                        {
                            // if not input layer in current network
                            if (layer > 0)
                            {
                                // add afferent edges to graph for current node
                                deep.AddEdges(net.GetInEdges(node).ToArray());
                            }
                            else
                            {
                                // add connections from previous network output layer to next input layer
                                foreach (var onode in prevOutput)
                                    deep.AddEdge(Edge.Create(onode, node));
                            }
                        }
                    }
                    // nodes in last layer
                    if (prevLayer != null)
                    {
                        // add outgoing connections for each node in previous layer
                        foreach (var inode in prevLayer)
                        {
                            deep.AddEdges(net.GetOutEdges(inode).ToArray());
                        }
                    }

                    // remember last layer
                    prevLayer = nodes.ToArray();
                }
                // remember last network output nodes
                prevOutput = deep.GetNodes(deep.Layers - 1).ToList();
            }

            deep.Reindex();

            deep.In = deep.GetNodes(0).ToArray();
            deep.Out = deep.GetNodes(deep.Layers - 1).ToArray();

            return deep;
        }
    }
}
