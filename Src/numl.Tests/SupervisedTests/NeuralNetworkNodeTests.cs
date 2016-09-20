using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Supervised.NeuralNetwork;
using numl.Math.LinearAlgebra;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class NeuralNetworkNodeTests : BaseSupervised
    {

        private readonly Matrix X = new double[][]
            {
                new [] {  1.0000,   2.0000,   3.0000,   4.0000  }, // X1
                new [] {  1.1000,   2.2000,   3.3000,   4.4000  }  // X2
            }.ToMatrix();
        private readonly Matrix y = new double[][]
        {
                new [] {  3.0000,   5.0000,   7.0000  }, // Y1
                new [] {  3.3000,   5.5000,   7.7000  }  // Y2
        }.ToMatrix();

        #region Weights

        private readonly Matrix Theta1 = new double[][]
        {              // bias       I1          I2          I3          I4
                new [] {   0.114406,   0.097015,   0.044837,   0.072042,   0.061151  }, // H1
                new [] {  -0.084899,   0.079426,   0.101367,  -0.116208,   0.036935  }, // H2
                new [] {   0.032989,   0.047999,   0.107589,   0.105116,   0.110863  }  // H2
        }.ToMatrix();

        private readonly Matrix Theta2 = new double[][]
        {             // bias         H1            H2          H3
                new [] {  0.0692946,  -0.0243359,   0.0597808,  -0.0645983  }, // O1
                new [] {  0.0802480,  -0.0777155,   0.0551302,  -0.0748193  }, // O2
                new [] { -0.0979650,   0.0079121,   0.0320823,  -0.0683478  }  // O3
        }.ToMatrix();

        private readonly Matrix Theta3 = new double[][]
        {             // bias         H1            H2          H3
                new [] { -0.042004,    0.045043,   -0.038244,    0.020853  }, // O1
                new [] {  0.077211,    0.017547,    0.079145,   -0.113062  }, // O2
                new [] {  0.092577,    0.047031,   -0.087198,    0.016963  }  // O3
        }.ToMatrix();

        #endregion

        #region Outputs

        private readonly Matrix Output_Out_1Layer = new double[][]
        {
                new [] {  0.50866,   0.49982,   0.46823  }, // Y1
                new [] {  0.50829,   0.49922,   0.46795  }  // Y2
        }.ToMatrix();

        private readonly Matrix Output_Out_2Layer = new double[][]
        {
                new [] {  0.49289,   0.51818,   0.52020  }, // Y1
                new [] {  0.49289,   0.51818,   0.52021  }  // Y2
        }.ToMatrix();

        #endregion

        #region 1 Layer Deltas

        private readonly Matrix Delta1_1Layer_Case1 = new double[][]
        {             // bias       I1          I2          I3          I4
               new [] {  0.077822,   0.077822,   0.155645,   0.233467,   0.311289  }, // H1
               new [] { -0.151646,  -0.151646,  -0.303291,  -0.454937,  -0.606583  }, // H2
               new [] {  0.180853,   0.180853,   0.361707,   0.542560,   0.723413  }  // H3
        }.ToMatrix();

        private readonly Matrix Delta1_1Layer_Case2 = new double[][]
        {              // bias       I1          I2          I3          I4
                new [] {  0.084555,   0.093011,   0.186022,   0.279033,   0.372044  }, // H1
                new [] { -0.168650,  -0.185515,  -0.371031,  -0.556546,  -0.742062  }, // H2
                new [] {  0.190853,   0.209938,   0.419876,   0.629814,   0.839753  }  // H3
        }.ToMatrix();

        private readonly Matrix Delta2_1Layer_Case1 = new double[][]
        {               // bias      H1        H2        H3
                new [] {   -2.4913,  -1.6985,  -1.2434,  -1.8479  }, // O1
                new [] {   -4.5002,  -3.0680,  -2.2460,  -3.3379  }, // O2
                new [] {   -6.5318,  -4.4530,  -3.2600,  -4.8448  }  // O3
        }.ToMatrix();

        private readonly Matrix Delta2_1Layer_Case2 = new double[][]
        {              // bias      H1        H2        H3
               new [] {   -2.7917,  -1.9420,  -1.3990,  -2.1240  },  // O1
               new [] {   -5.0008,  -3.4787,  -2.5060,  -3.8047  },  // O2
               new [] {   -7.2321,  -5.0308,  -3.6242,  -5.5023  }   // O3
        }.ToMatrix();

        private readonly Matrix Delta_Out_1Layer = new double[][]
        {             // O1        O2        O3
               new [] {  -2.4913,  -4.5002,  -6.5318  }, // Y1
               new [] {  -2.7917,  -5.0008,  -7.2321  }, // Y2
        }.ToMatrix();

        #endregion

        #region 2 Layer Deltas

        private readonly Matrix Delta1_2Layer_Case1 = new double[][]
        {              // bias       I1          I2          I3          I4
                new [] {  -4.8851e-004,  -4.8851e-004,  -9.7702e-004,  -1.4655e-003,  -1.9540e-003  }, // H1
                new [] {  -1.1079e-004,  -1.1079e-004,  -2.2157e-004,  -3.3236e-004,  -4.4314e-004  }, // H2
                new [] {  -6.8532e-004,  -6.8532e-004,  -1.3706e-003,  -2.0560e-003,  -2.7413e-003  }  // H3
        }.ToMatrix();

        private readonly Matrix Delta1_2Layer_Case2 = new double[][]
        {              // bias       I1          I2          I3          I4
                new [] {  -5.2509e-004,  -5.7760e-004,  -1.1552e-003,  -1.7328e-003,  -2.3104e-003  }, // H1
                new [] {  -1.2692e-004,  -1.3961e-004,  -2.7923e-004,  -4.1884e-004,  -5.5845e-004  }, // H2
                new [] {  -7.1971e-004,  -7.9168e-004,  -1.5834e-003,  -2.3751e-003,  -3.1667e-003  }  // H3
        }.ToMatrix();

        private readonly Matrix Delta2_2Layer_Case1 = new double[][]
        {               // bias      H1        H2        H3
                new [] {   -0.124043,  -0.084566,  -0.061909,  -0.092006  }, // O1
                new [] {    0.076548,   0.052186,   0.038205,   0.056778  }, // O2
                new [] {    0.085783,   0.058483,   0.042814,   0.063628 }   // O3
        }.ToMatrix();

        private readonly Matrix Delta2_2Layer_Case2 = new double[][]
        {               // bias      H1        H2        H3
                new [] {   -0.137844,  -0.095888,  -0.069077,  -0.104874  }, // O1
                new [] {    0.084782,   0.058977,   0.042486,   0.064504  }, // O2
                new [] {    0.095338,   0.066320,   0.047776,   0.072535  }   // O3
        }.ToMatrix();

        private readonly Matrix Delta3_2Layer_Case1 = new double[][]
        {               // bias      H1        H2        H3
                new [] {   -2.5071,  -1.2753,  -1.2531,  -1.1739  }, // O1
                new [] {   -4.4818,  -2.2797,  -2.2401,  -2.0985  }, // O2
                new [] {   -6.4798,  -3.2960,  -3.2387,  -3.0340  }  // O3
        }.ToMatrix();

        private readonly Matrix Delta3_2Layer_Case2 = new double[][]
        {               // bias      H1        H2        H3
                new [] {   -2.8071,  -1.4268,  -1.4014,  -1.3136  }, // O1
                new [] {   -4.9818,  -2.5322,  -2.4870,  -2.3312  }, // O2
                new [] {   -7.1798,  -3.6494,  -3.5843,  -3.3598  }  // O3
        }.ToMatrix();

        private readonly Matrix Delta_Out_2Layer = new double[][]
        {             // O1        O2        O3
               new [] {  -2.5071,  -4.4818,  -6.4798  }, // Y1
               new [] {  -2.8071,  -4.9818,  -7.1798  }, // Y2
        }.ToMatrix();

        #endregion

        /// <summary>
        /// Function for updating the specified theta value and returning the new error.
        /// </summary>
        private readonly Func<Network, Neuron, Vector, Vector, int, double, double> FnCostUpdateFunction = (network, n, input, output, id, theta) =>
        {
            n.Out[id].Weight = theta;

            network.Forward(input);
            network.Back(output, null, false);

            return network.Cost;
        };

        private Network Get_1Layer_Network()
        {
            return Network.Create(4, 3, new Math.Functions.Logistic(), null, null, (l, i, j) =>
            {
                if (l == 0) return Theta1[j - 1, i];
                else return Theta2[j, i];
            }, hiddenLayers: new int[] { 3 });
        }

        private Network Get_2Layer_Network()
        {
            return Network.Create(4, 3, new Math.Functions.Logistic(), null, null, (l, i, j) =>
            {
                if (l == 0) return Theta1[j - 1, i];
                else if (l == 1) return Theta2[j - 1, i];
                else return Theta3[j, i];
            }, hiddenLayers: new int[] { 3, 3 });
        }

        [Fact]
        public void Network_1Layer_Forward_Sigmoid_Test()
        {
            var net = Get_1Layer_Network();

            for (int row = 0; row < X.Rows; row++)
            {
                net.Forward(X[row, VectorType.Row]);

                for (int output = 0; output < net.Out.Length; output++)
                {
                    Almost.Equal(Output_Out_1Layer[row, output], net.Out[output].Output, 0.0001);
                }
            }
        }

        [Fact]
        public void Network_1Layer_Backward_Sigmoid_Test()
        {
            var net = Get_1Layer_Network();

            var hiddenNodes = net.GetNodes(1);

            for (int row = 0; row < X.Rows; row++)
            {
                Console.WriteLine($"Evaluating Pass {row}...");

                net.Forward(X[row, VectorType.Row]);
                net.Back(y[row, VectorType.Row], null, false);

                for (int output = 0; output < net.Out.Count(); output++)
                {
                    Almost.Equal(Delta_Out_1Layer[row, output], net.Out.ElementAt(output).Delta, 0.0001);
                }

                for (int hidden = 0; hidden < hiddenNodes.Count(); hidden++)
                {
                    if (row == 0)
                        Almost.Equal(Delta2_1Layer_Case1[hidden, VectorType.Col].Sum(), hiddenNodes.ElementAt(hidden).Delta, 0.0001,
                            $"Node: {hiddenNodes.ElementAt(hidden).Label}");
                    else
                        Almost.Equal(Delta2_1Layer_Case2[hidden, VectorType.Col].Sum(), hiddenNodes.ElementAt(hidden).Delta, 0.0001,
                            $"Node: {hiddenNodes.ElementAt(hidden).Label}");
                }

                for (int input = 0; input < net.In.Length; input++)
                {
                    if (row == 0)
                        Almost.Equal(Delta1_1Layer_Case1[input, VectorType.Col].Sum(), net.In[input].Delta, 0.2,
                            $"Node: {net.In[input].Label}");
                    else
                        Almost.Equal(Delta1_1Layer_Case2[input, VectorType.Col].Sum(), net.In[input].Delta, 0.2,
                            $"Node: {net.In[input].Label}");
                }
            }
        }

        [Fact]
        public void Network_2Layer_Forward_Sigmoid_Test()
        {
            var net = Get_2Layer_Network();

            for (int row = 0; row < X.Rows; row++)
            {
                net.Forward(X[row, VectorType.Row]);

                for (int output = 0; output < net.Out.Length; output++)
                {
                    Almost.Equal(Output_Out_2Layer[row, output], net.Out[output].Output, 0.0025);
                }
            }
        }

        [Fact]
        public void Network_2Layer_Backward_Sigmoid_Test()
        {
            var net = Get_2Layer_Network();

            var hiddenNodes2 = net.GetNodes(1);
            var hiddenNodes3 = net.GetNodes(2);

            for (int row = 0; row < X.Rows; row++)
            {
                Console.WriteLine($"Evaluating Pass {row}...");

                net.Forward(X[row, VectorType.Row]);
                net.Back(y[row, VectorType.Row], null, false);

                for (int output = 0; output < net.Out.Count(); output++)
                {
                    Almost.Equal(Delta_Out_2Layer[row, output], net.Out.ElementAt(output).Delta, 0.005);
                }

                for (int hidden = 0; hidden < hiddenNodes3.Count(); hidden++)
                {
                    if (row == 0)
                        Almost.Equal(Delta3_2Layer_Case1[hidden, VectorType.Col].Sum(), hiddenNodes3.ElementAt(hidden).Delta, 0.005,
                            $"Node: {hiddenNodes3.ElementAt(hidden).Label}");
                    else
                        Almost.Equal(Delta3_2Layer_Case2[hidden, VectorType.Col].Sum(), hiddenNodes3.ElementAt(hidden).Delta, 0.005,
                            $"Node: {hiddenNodes3.ElementAt(hidden).Label}");
                }

                for (int hidden = 0; hidden < hiddenNodes2.Count(); hidden++)
                {
                    if (row == 0)
                        Almost.Equal(Delta2_2Layer_Case1[hidden, VectorType.Col].Sum(), hiddenNodes2.ElementAt(hidden).Delta, 0.05,
                            $"Node: {hiddenNodes2.ElementAt(hidden).Label}");
                    else
                        Almost.Equal(Delta2_2Layer_Case2[hidden, VectorType.Col].Sum(), hiddenNodes2.ElementAt(hidden).Delta, 0.05,
                            $"Node: {hiddenNodes2.ElementAt(hidden).Label}");
                }

                for (int input = 0; input < net.In.Length; input++)
                {
                    if (row == 0)
                        Almost.Equal(Delta1_2Layer_Case1[input, VectorType.Col].Sum(), net.In[input].Delta, 0.8,
                            $"Node: {net.In[input].Label}");
                    else
                        Almost.Equal(Delta1_2Layer_Case2[input, VectorType.Col].Sum(), net.In[input].Delta, 0.8,
                            $"Node: {net.In[input].Label}");
                }
            }
        }

        [Fact]
        public void Check_Neural_Network_Gradients()
        {
            Matrix xtest = new double[][]
            {
                new [] {    0.54030,  -0.41615  }, // X1
                new [] {    0.54030,  -0.41615  }, // X2
                new [] {    0.28366,   0.96017  }  // X3
            }.ToMatrix();

            Matrix ytest = new double[][]
            {
                new [] {   0d, 0d, 0d, 1d  }, // X1
                new [] {   0d, 1d, 0d, 0d  }, // X2
                new [] {   0d, 0d, 1d, 0d  }  // X3
            }.ToMatrix();

            Matrix theta1 = new double[][]
            {              //b  //1  //2
                new [] {   0.1, 0.3, 0.5  }, // h1
                new [] {   0.2, 0.4, 0.6  }, // h2
            }.ToMatrix();

            Matrix theta2 = new double[][]
            {              //b //h1 //h2
                new [] {   0.7, 1.1, 1.5  }, //o1
                new [] {   0.8, 1.2, 1.6  }, //o2
                new [] {   0.9, 1.3, 1.7  }, //o3
                new [] {   1.0, 1.4, 1.8  }, //o4
            }.ToMatrix();

            Matrix delta1 = new double[][]
            {              //b        //1       //2
                new [] {   0.79393,   0.42896,  -0.33039  }, //h1
                new [] {   1.05281,   0.56883,  -0.43812  }, //h2
            }.ToMatrix();

            Matrix delta2 = new double[][]
            {              //b         //h1        //h2
                new [] {   0.888659,   0.456328,   0.481220  }, //O1
                new [] {   0.907427,   0.465965,   0.491383  }, //O2
                new [] {   0.923305,   0.474118,   0.499981  }, //O3
                new [] {  -0.063351,  -0.032531,  -0.034305  }, //O4
            }.ToMatrix();

            Vector delta3 = new[] { 0.888659,  0.907427,  0.923305,  -0.063351 };

            Network net = Network.Create(2, 4, new numl.Math.Functions.Logistic(), fnWeightInitializer: (l, i, j) =>
            {
                if (l == 1)
                {
                    return theta2[j, i];
                }
                else
                {
                    return theta1[j - 1, i];
                }
            }, hiddenLayers: 2);

            net.Forward(xtest[0, VectorType.Row]);
            net.Back(ytest[0, VectorType.Row], null, false);

            var hiddenNodes = net.GetNodes(1);

            for (int output = 0; output < net.Out.Count(); output++)
            {
                Almost.Equal(delta3[output], net.Out.ElementAt(output).Delta, 0.0001);
            }

            for (int hidden = 0; hidden < hiddenNodes.Count(); hidden++)
            {
                Almost.Equal(delta2[hidden, VectorType.Col].Sum(), hiddenNodes.ElementAt(hidden).Delta, 0.0001,
                        $"Node: {hiddenNodes.ElementAt(hidden).Label}");
            }
        }
    }
}
