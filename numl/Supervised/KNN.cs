using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Math.LinearAlgebra;
using System.Threading.Tasks;

namespace numl.Supervised
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

            //for (int i = 0; i < X.RowCount; i++) // distance (y - x[i]).Norm
            //    distances[i] = new Tuple<int, double>(i, (y - X.Row(i)).Norm(2));

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
