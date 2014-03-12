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
            if (Output == -1)
                Output = y.Distinct().Count();
            // set number of hidden units to 
            // (Input + Hidden) * 2/3 as basic
            // best guess
            if (Hidden == -1)
                Hidden = (int)System.Math.Ceiling((decimal)(x.Cols + Output) * 2m / 3m);



            throw new NotImplementedException();
        }
    }
}
