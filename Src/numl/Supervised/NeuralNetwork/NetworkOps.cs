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

            // creating input nodes
            network.In = new Neuron[x.Cols + 1];
            network.In[0] = network.AddVertex(new Neuron(true) { Label = "B0", ActivationFunction = ident, NodeId = 0, LayerId = 0 });
            for (int i = 1; i < x.Cols + 1; i++)
                network.In[i] = network.AddVertex(new Neuron { Label = d.ColumnAt(i - 1), ActivationFunction = ident, NodeId = i, LayerId = 0 });

            // creating hidden nodes
            Neuron[] h = new Neuron[hidden + 1];
            h[0] = network.AddVertex(new Neuron(true) { Label = "B1", ActivationFunction = ident, NodeId = 0, LayerId = 1 });
            for (int i = 1; i < hidden + 1; i++)
                h[i] = network.AddVertex(new Neuron {
                    Label = String.Format("H{0}", i),
                    ActivationFunction = activationFunction,
                    OutputFunction = outputFunction,
                    NodeId = i,
                    LayerId = 1
                });

            // creating output nodes
            network.Out = new Neuron[output];
            for (int i = 0; i < output; i++)
                network.Out[i] = network.AddVertex(new Neuron {
                    Label = Network.GetLabel(i, d),
                    ActivationFunction = activationFunction,
                    OutputFunction = outputFunction,
                    NodeId = i,
                    LayerId = 2
                });

            // link input to hidden. Note: there are
            // no inputs to the hidden bias node
            for (int i = 1; i < h.Length; i++)
                for (int j = 0; j < network.In.Length; j++)
                    network.AddEdge(Edge.Create(network.In[j], h[i], epsilon: epsilon));

            // link from hidden to output (full)
            for (int i = 0; i < network.Out.Length; i++)
                for (int j = 0; j < h.Length; j++)
                    network.AddEdge(Edge.Create(h[j], network.Out[i], epsilon: epsilon));

            return network;
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

            if (fnNodeInitializer == null)
                fnNodeInitializer = new Func<int, int, Neuron>((i, j) => new Neuron());

            if (fnWeightInitializer == null)
                fnWeightInitializer = new Func<int, int, int, double>((l, i, j) => double.NaN);

            // creating input nodes
            network.In = new Neuron[inputLayer + 1];
            network.In[0] = network.AddVertex(new Neuron(true) { Label = "B0", ActivationFunction = ident, NodeId = 0, LayerId = 0 });

            for (int i = 1; i < inputLayer + 1; i++)
            {
                network.In[i] = fnNodeInitializer(0, i);
                network.In[i].Label = (network.In[i].Label ?? string.Format("I{0}", i));
                network.In[i].ActivationFunction = (network.In[i].ActivationFunction ?? ident);
                network.In[i].LayerId = 0;
                network.In[i].NodeId = i;

                network.AddVertex(network.In[i]);
            }

            Neuron[] last = null;
            for (int layerIdx = 0; layerIdx < hiddenLayers.Length; layerIdx++)
            {
                // creating hidden nodes
                Neuron[] layer = new Neuron[hiddenLayers[layerIdx] + 1];
                layer[0] = network.AddVertex(new Neuron(true) { Label = "B1", ActivationFunction = ident, LayerId = layerIdx + 1, NodeId = 0 });
                for (int i = 1; i < layer.Length; i++)
                {
                    layer[i] = fnNodeInitializer(layerIdx + 1, i);
                    layer[i].Label = (layer[i].Label ?? String.Format("H{0}.{1}", layerIdx + 1, i));
                    layer[i].ActivationFunction = (layer[i].ActivationFunction ?? activationFunction);
                    layer[i].OutputFunction = layer[i].OutputFunction;
                    layer[i].LayerId = layerIdx + 1;
                    layer[i].NodeId = i;

                    network.AddVertex(layer[i]);
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

                network.AddVertex(network.Out[i]);
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
                network.AddVertex(node);

            int hiddenLayer = inputLayerId + 1;
            // relink nodes
            Neuron[] last = null;
            for (int layerIdx = hiddenLayer; layerIdx < outputLayerId; layerIdx++)
            {
                Neuron[] layer = nodes.Where(w => w.LayerId == layerIdx).ToArray();

                foreach (var node in layer)
                    network.AddVertex(node);

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
                network.AddVertex(node);

            for (int i = 0; i < network.Out.Length; i++)
                for (int j = 0; j < last.Length; j++)
                    network.AddEdge(Edge.Create(last[j], network.Out[i], 
                        weight: edges.First(f => f.ParentId == last[j].Id && f.ChildId == network.Out[i].Id).Weight));

            return network;
        }
    }
}
