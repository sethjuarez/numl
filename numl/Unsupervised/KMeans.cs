/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using numl.Math.Metrics;
using numl.Model;
using numl.Math;
using numl.Math.Probability;
using numl;

namespace ml.Unsupervised
{
    public class KMeans
    {
        public Description Description { get; set; }
        public Matrix Centers { get; set; }

        public int[] Generate(IEnumerable<object> examples, int k, IDistance metric = null)
        {
            #region Setup
            if (examples == null)
                throw new InvalidOperationException("Cannot generate a model will no data!");

            if (k < 2)
                throw new InvalidOperationException("Can only cluter with k > 1");

            if (Description == null)
                throw new InvalidOperationException("Invalid Description!");

            Matrix X = examples.ToMatrix(Description);

            // clear out zeros (if any...)
            // var indices = X.Indices(v => !v.All(d => d == 0), VectorType.Row);
            // X = X.Slice(indices, VectorType.Row);

            // X.Normalize(VectorType.Row);

            if (k >= X.Rows)
                throw new InvalidOperationException(
                    string.Format("Cannot cluster {0} items {1} different ways!", X.Rows, k));

            var means = InitializeRandom(X, k);
            var diff = double.MaxValue;
            int[] assignments = new int[X.Rows];
            #endregion

            if (metric == null)
                metric = new EuclidianDistance();

            for (int i = 0; i < 100; i++)
            {
                // Assignment step
                Parallel.For(0, X.Rows, j =>
                //for (int j = 0; j < X.Rows; j++)
                {
                    var min_index = -1;
                    var min = double.MaxValue;
                    // current example
                    var x = X[j, VectorType.Row];
                    for (int m = 0; m < means.Rows; m++)
                    {
                        var d = means[m, VectorType.Row];
                        var distance = metric.Compute(x, d);
                        if (distance < min)
                        {
                            min = distance;
                            min_index = m;
                        }
                        assignments[j] = min_index;
                    }
                //}
                });

                // Update Step

                // new means has k rows and X.Cols columns
                Matrix new_means = Matrix.Zeros(k, X.Cols);
                Vector sum = Vector.Zeros(k);

                // Part 1: Sum up assignments
                for (int j = 0; j < X.Rows; j++)
                {
                    new_means[assignments[j], VectorType.Row] += X[j, VectorType.Row];
                    sum[assignments[j]]++;
                }

                // Part 2: Divide by counts
                for(int j = 0; j <new_means.Rows; j++)
                    new_means[j, VectorType.Row] /= sum[j];

                // find sum of normdiff's of means
                diff = means.GetRows()
                        .Zip(new_means.GetRows(), (v1, v2) => new { V1 = v1, V2 = v2 })
                        .Sum(a => Vector.NormDiff(a.V1, a.V2));

                // small diff? return
                if (diff < .00001)
                {
                    Centers = means;
                    return assignments;
                }

                means = new_means;
            }

            Centers = means;
            return assignments;
        }

        private Matrix InitializeUniform(Matrix X, int k)
        {
            int multiple = (int)Math.Floor((double)X.Rows / (double)k);

            var m = Matrix.Zeros(k, X.Cols);

            for (int i = 0; i < k; i++)
                m[i, VectorType.Row] = X[i * multiple, VectorType.Row];

            return m;

        }

        private Matrix InitializeRandom(Matrix X, int k)
        {
            // initialize mean variables
            // to random existing points
            var m = Matrix.Zeros(k, X.Cols);

            var seeds = new List<int>(k);
            for (int i = 0; i < k; i++)
            {
                int index = -1;
                do
                {
                    // pick random row that has not yet 
                    // been used
                    index = MLRandom.GetUniform(k);

                    if (!seeds.Contains(index))
                    {
                        seeds.Add(index);
                        break;
                    }

                }
                while (true);
                m[i, VectorType.Row] = X[index, VectorType.Row];
            }

            return m;
        }
    }
}
