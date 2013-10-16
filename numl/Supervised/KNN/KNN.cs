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

    public class KNNModel : Model
    {
        public int K { get; set; }
        public Matrix X { get; set; }
        public Vector Y { get; set; }

        public override double Predict(Vector y)
        {
            Tuple<int, double>[] distances = new Tuple<int, double>[y.Length];

            // happens per slot so we are good to parallelize
            Parallel.For(0, X.Rows, i => distances[i] = new Tuple<int, double>(i, (y - X.Row(i)).Norm(2)));

            var slice = distances
                            .OrderBy(t => t.Item2)
                            .Take(K)
                            .Select(i => i.Item1);

            return Y.Slice(slice).Mode();
        }
    }
}
