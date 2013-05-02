﻿using System;
using numl.Model;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.Probability;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    public class KMeans
    {
        public Descriptor Descriptor { get; set; }
        public Matrix Centers { get; set; }

        public KMeans()
        {

        }

        public Cluster Generate(Descriptor descriptor, IEnumerable<object> examples, int k, IDistance metric = null)
        {
            var data = examples.ToArray();
            Descriptor = descriptor;
            Matrix X = Descriptor.Convert(examples).ToMatrix();

            var assignments = Generate(X, k, metric);

            return GenerateClustering(X, assignments, data);

        }


        private Cluster GenerateClustering(Matrix x, int[] assignments, object[] data = null)
        {
            var clusters = new List<Cluster>();

            // Create a new cluster for each data point
            for (int i = 0; i < x.Rows; i++)
                clusters.Add(new Cluster
                {
                    Id = i,
                    Points = new Vector[] { x[i] },
                    Members = data != null ? new object[] { data[i] } : new object[] { x[i] }
                });

            // TODO: FINISH HERE!
            throw new NotImplementedException();
        }

        public int[] Generate(Matrix X, int k, IDistance metric, object[] data = null)
        {
            if (metric == null)
                metric = new EuclidianDistance();

            var means = InitializeRandom(X, k);
            int[] assignments = new int[X.Rows];

            for (int i = 0; i < 100; i++)
            {
                // Assignment step
                Parallel.For(0, X.Rows, j =>
                {
                    var min_index = -1;
                    var min = double.MaxValue;
                    for (int m = 0; m < means.Rows; m++)
                    {
                        var d = metric.Compute(X[j], means[m]);
                        if (d < min)
                        {
                            min = d;
                            min_index = m;
                        }
                    }

                    assignments[j] = min_index;
                });

                // Update Step
                Matrix new_means = Matrix.Zeros(k, X.Cols);
                Vector sum = Vector.Zeros(k);

                // Part 1: Sum up assignments
                for (int j = 0; j < X.Rows; j++)
                {
                    int a = assignments[j];
                    new_means[a] += X[j];
                    sum[a]++;
                }

                // Part 2: Divide by counts
                for (int j = 0; j < new_means.Rows; j++)
                    new_means[j] /= sum[j];

                // Part 3: Check for convergence
                // find norm of the difference
                if ((means - new_means).Norm() < .00001)
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
            int multiple = (int)System.Math.Floor((double)X.Rows / (double)k);

            var m = Matrix.Zeros(k, X.Cols);

            for (int i = 0; i < k; i++)
                for (int j = 0; j < X.Cols; j++)
                    m[i, j] = X[i * multiple, j];

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
                    // been used (need to fix this...)

                    index = Sampling.GetUniform(k);

                    if (!seeds.Contains(index))
                    {
                        seeds.Add(index);
                        break;
                    }

                }
                while (true);

                for (int j = 0; j < X.Cols; j++)
                    m[i, j] = X[index, j];
            }

            return m;
        }
    }
}
