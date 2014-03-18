using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Supervised.NeuralNetwork
{
    public class Node
    {
        public double Output { get; set; }
        public double Input { get; set; }
        public double Error { get; set; }
        public Edge[] Out { get; set; }
        public Edge[] In { get; set; }
    }

    public class Edge
    {
        public Node Source { get; set; }
        public Node Target { get; set; }
        public double Weight { get; set; }
    }

    public class Network
    {
        public Node[] In { get; set; }
        public Node[] Out { get; set; }

        public static Network Default(Matrix x, Vector y, IFunction activation)
        {
            // set output to number of
            // choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;

            // set number of hidden units to 
            // (Input + Hidden) * 2/3 as basic
            // best guess
            int hidden = (int)System.Math.Ceiling((decimal)(x.Cols + output) * 2m / 3m);




            throw new NotImplementedException();
        }

        public void Evaluate(Vector x)
        {
            if(In.Length != x.Length + 1)
                throw new InvalidOperationException("Input nodes not aligned to input vector");

        }
    }
}
