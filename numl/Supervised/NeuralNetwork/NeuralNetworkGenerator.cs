using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;

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
            // set output to number of
            // choices of available
            // 1 if only two choices
            if (Output == -1)
            {
                int distinct = y.Distinct().Count();
                Output = distinct > 2 ? distinct : 1;
            }

            // set number of hidden units to 
            // (Input + Hidden) * 2/3 as basic
            // best guess
            if (Hidden == -1)
                Hidden = (int)System.Math.Ceiling((decimal)(x.Cols + Output) * 2m / 3m);

            // initial weight vector (add one for bias)
            Vector W = Vector.Ones(x.Cols + 1);
            // activation layer
            Vector A = Vector.Ones(Hidden);
            // activation output
            Vector Z = Vector.Ones(Hidden);
            // final output
            Vector Y = Vector.Ones(Output);

            // forward propagation




            throw new NotImplementedException();
        }
    }
}
