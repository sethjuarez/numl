using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Utils;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Supervised.NeuralNetwork;

using Xunit;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class NeuralNetworkStructuralTests
    {
        private static Network GetNetwork(int inputSize, int outputSize, int[] hiddenSizes)
        {
            return Network.New().Create(inputSize, outputSize,
                                        activationFunction: new RectifiedLinear(),
                                        outputFunction: null,
                                        fnWeightInitializer: null,
                                        epsilon: 0.08,
                                        hiddenLayers: hiddenSizes);
        }

        private static int VerifyNetwork(Network network)
        {
            int errors = 0;

            var edges = network.GetEdges();

            for (int layer = 0; layer < network.Layers; layer++)
            {
                var nodes = network.GetNodes(layer);

                foreach (var node in nodes)
                {
                    foreach (var iedge in node.In)
                    {
                        if (!edges.Any(a => a.ParentId == iedge.ParentId && a.ChildId == iedge.ChildId))
                            errors++;
                    }
                    foreach (var oedge in node.Out)
                    {
                        if (!edges.Any(a => a.ParentId == oedge.ParentId && a.ChildId == oedge.ChildId))
                            errors++;
                    }

                    if (layer == 0 && node.In.Count > 0) errors++;
                    if (layer == network.Layers - 1 && node.Out.Count > 0) errors++;
                }
            }

            network.Forward(Vector.Ones(network.In.Length - 1));

            if (network.Out.Any(a => double.IsNaN(a.Output) || double.IsInfinity(a.Output))) errors++;

            return errors;
        }

        [Theory]
        [InlineData(true, 2, 10, 8, new int[] { 4, 3, 1 })]
        [InlineData(true, 3, 8, 8, new int[] { 4, 4, 4, 4 })]
        [InlineData(false, 2, 10, 8, new int[] { 4, 3, 1 })]
        [InlineData(false, 3, 8, 8, new int[] { 4, 4, 4, 4 })]
        public void Network_Prune_Test(bool backwards, int pruneLayers, int input, int output, int[] hidden)
        {
            Network network = GetNetwork(input, output, hidden);

            List<int> layers = new List<int>();
            layers.Add(input);
            layers.AddRange(hidden);
            layers.Add(output);

            int newInput = (backwards ? layers[0] + 1 : layers[pruneLayers] + 1);
            int newOutput = (backwards ? layers[layers.Count - (pruneLayers + 1)] : layers[layers.Count - 1]);
            int[] newHidden = layers.Skip(backwards ? 1 : pruneLayers + 1)
                                    .Take(layers.Count - (pruneLayers + 2)).ToArray();

            List<int> layers2 = new List<int>();
            layers2.Add(newInput);

            for (int l = 0; l < newHidden.Length; l++)
                layers2.Add(newHidden[l] + 1);

            layers2.Add(newOutput);

            network.Prune(backwards, pruneLayers);

            Assert.Equal(newInput, network.In.Length);
            Assert.Equal(newOutput, network.Out.Length);

            for (int x = 0; x < layers2.Count; x++)
            {
                Assert.Equal(layers2[x], network.GetNodes(x).Count());
            }

            Assert.Equal(layers2.Sum(), network.GetVertices().Count());

            Assert.True(VerifyNetwork(network) == 0);
        }

        [Theory]
        [InlineData(8, new int[] { 5 }, 4, 3, new int[] { 2 }, 1, true, true)]
        [InlineData(90, new int[] { 10, 8, 6 }, 5, 7, new int[] { 4, 2, 3 }, 10, true, true)]
        [InlineData(70, new int[] { 10, 8, 6 }, 5, 7, new int[] { 4, 2, 3 }, 10, true, false)]
        [InlineData(50, new int[] { 10, 8, 6 }, 5, 7, new int[] { 4, 2, 3 }, 10, false, true)]
        [InlineData(30, new int[] { 10, 8, 6 }, 5, 7, new int[] { 4, 2, 3 }, 10, false, false)]
        public void Network_Stack_1_Test(int input1, int[] hidden1, int output1, int input2, int[] hidden2, int output2, bool pruneInputs, bool pruneOutputs)
        {
            List<int> layers = new List<int>();

            layers.Add(input1 + 1); layers.AddRange(hidden1.Select(s => s + 1));
            if (!pruneOutputs) layers.Add(output1 + 1);

            if (!pruneInputs) layers.Add(input2 + 1);
            layers.AddRange(hidden2.Select(s => s + 1)); layers.Add(output2);

            Network net1 = GetNetwork(input1, output1, hidden1);
            Network net2 = GetNetwork(input2, output2, hidden2);

            Network deep = net1.Stack(pruneInputs, pruneOutputs, true, true, net2);

            for (int x = 0; x < layers.Count; x++)
            {
                Assert.Equal(layers[x], deep.GetNodes(x).Count());
            }

            Assert.True(VerifyNetwork(deep) == 0);
        }
    }
}
