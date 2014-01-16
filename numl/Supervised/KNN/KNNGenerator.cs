using System;
using System.Linq;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.KNN
{
    public class KNNGenerator : Generator
    {
        public int K { get; set; }

        public KNNGenerator(int k = 5)
        {
            K = k;
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            return new KNNModel
            {
                X = x,
                Y = y,
                K = K
            };
        }
    }
}