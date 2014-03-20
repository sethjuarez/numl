using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork
{
    public class NeuralNetworkGenerator : Generator
    {
        public int Hidden { get; private set; }
        public int Output { get; private set; }
        

        public NeuralNetworkGenerator(int hidden = -1, int output = -1)
        {
            Output = output;
            Hidden = hidden;
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            var network = Network.Default(Descriptor, x, y, new Tanh());

            for (int i = 0; i < x.Rows; i++)
            {
                network.Evaluate(x[i, VectorType.Row]);
                network.Backprop(y[i]);
            }

            throw new NotImplementedException();
        }
    }
}
