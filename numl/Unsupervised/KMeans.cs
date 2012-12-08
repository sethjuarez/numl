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
using numl.Model;
using System.Linq;
using numl.Math.Metrics;
using numl.Math;
using System.Threading.Tasks;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Unsupervised
{
    public class KMeans
    {
        public Descriptor Descriptor { get; set; }
        public Matrix Centers { get; set; }

        public KMeans()
        {
            
        }

        public KMeans(Descriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public int[] Generate(Matrix X, int k, IDistance metric = null)
        {
            if (metric == null)
                metric = new EuclidianDistance();

            var means = InitializeRandom(X, k);
            var diff = double.MaxValue;
            int[] assignments = new int[X.RowCount];

            for (int i = 0; i < 100; i++)
            {
                // Assignment step
                Parallel.For(0, X.RowCount, j =>
                {
                    var min_index = -1;
                    var min = double.MaxValue;
                    // current example
                    var x = (Vector)X.Row(j);
                    for (int m = 0; m < means.RowCount; m++)
                    {
                        var d = (Vector)means.Row(m);
                        var distance = metric.Compute(x, d);
                        if (distance < min)
                        {
                            min = distance;
                            min_index = m;
                        }
                        assignments[j] = min_index;
                    }
                });

                // Update Step
                // new means has k rows and X.Cols columns
                Matrix new_means = new DenseMatrix(k, X.ColumnCount, 0);
                Vector sum = new DenseVector(k, 0);

                // Part 1: Sum up assignments
                for (int j = 0; j < X.RowCount; j++)
                {
                    for (int z = 0; z < X.ColumnCount; z++)
                        new_means[j, z] += X.At(j, z);
                    sum[assignments[j]]++;
                }

                // Part 2: Divide by counts
                for (int j = 0; j < new_means.RowCount; j++)
                    for(int z= 0; z < new_means.ColumnCount; z++)
                        new_means[j, z] /= sum.At(j);


                // Part 3: Check for convergence
                // find sum of normdiff's of means
                means.RowEnumerator()
                    .Zip(new_means.RowEnumerator(), (e1, e2) => new { V1 = e1.Item2, V2 = e2.Item2 })
                    .Sum(a => (a.V1 - a.V2).Norm(2));


                // small diff? return
                if (diff < .00001)
                    break;

                means = new_means;
            }

            Centers = means;

            return assignments;
        }

        public int[] Generate(IEnumerable<object> examples, int k, IDistance metric = null)
        {
            #region Sanity Checks
            if (examples == null)
                throw new InvalidOperationException("Cannot generate a model will no data!");

            if (k < 2)
                throw new InvalidOperationException("Can only cluter with k > 1");

            if (Descriptor == null)
                throw new InvalidOperationException("Invalid Description!");

            int count = examples.Count();
            if (k >= count)
                throw new InvalidOperationException(
                    string.Format("Cannot cluster {0} items {1} different ways!", count, k));
            #endregion

            Matrix X = Descriptor.Convert(examples).ToMatrix();
            var data = Generate(X, k, metric);
            return data;
        }

        private Matrix InitializeUniform(Matrix X, int k)
        {
            int multiple = (int)System.Math.Floor((double)X.RowCount / (double)k);

            var m = new DenseMatrix(k, X.ColumnCount, 0);

            for (int i = 0; i < k; i++)
                for (int j = 0; j < X.ColumnCount; j++)
                    m[i, j] = X.At(i * multiple, j);

            return m;

        }

        private Matrix InitializeRandom(Matrix X, int k)
        {
            // initialize mean variables
            // to random existing points
            var m = new DenseMatrix(k, X.ColumnCount, 0);
            Random r = new Random(DateTime.Now.Millisecond);
            r.Next(k);

            var seeds = new List<int>(k);
            for (int i = 0; i < k; i++)
            {
                int index = -1;
                do
                {
                    // pick random row that has not yet 
                    // been used (need to fix this...)

                    index = r.Next(k);

                    if (!seeds.Contains(index))
                    {
                        seeds.Add(index);
                        break;
                    }

                }
                while (true);

                for (int j = 0; j < X.ColumnCount; j++)
                    m[i, j] = X.At(index, j);
            }

            return m;
        }
    }
}
