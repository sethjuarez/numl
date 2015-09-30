// file:	Supervised\KNN\KNNModel.cs
//
// summary:	Implements the knn model class
using System;
using System.Linq;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.KNN
{
    /// <summary>A data Model for the knn.</summary>
    public class KNNModel : Model
    {
        /// <summary>Gets or sets the k.</summary>
        /// <value>The k.</value>
        public int K { get; set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        public Matrix X { get; set; }
        /// <summary>Gets or sets the y coordinate.</summary>
        /// <value>The y coordinate.</value>
        public Vector Y { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            Tuple<int, double>[] distances = new Tuple<int, double>[X.Rows];

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
