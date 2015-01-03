// file:	Unsupervised\KMeans.cs
//
// summary:	Implements the means class
using System;
using numl.Model;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.Probability;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    /// <summary>A means.</summary>
    public class KMeans
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }
        /// <summary>Gets or sets the centers.</summary>
        /// <value>The centers.</value>
        public Matrix Centers { get; set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        public Matrix X { get; set; }
        /// <summary>Generates.</summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="examples">The examples.</param>
        /// <param name="k">The int to process.</param>
        /// <param name="metric">(Optional) the metric.</param>
        /// <returns>An int[].</returns>
        public Cluster Generate(Descriptor descriptor, IEnumerable<object> examples, int k, IDistance metric = null)
        {
            var data = examples.ToArray();
            Descriptor = descriptor;
            X = Descriptor.Convert(data).ToMatrix();

            // generate assignments
            var assignments = Generate(X, k, metric);

            // begin packing objects into clusters
            var objects = new List<object>[k];
            for (int i = 0; i < assignments.Length; i++)
            {
                var a = assignments[i];
                if (objects[a] == null) objects[a] = new List<object>();
                objects[a].Add(data[i]);
            }

            // create clusters
            List<Cluster> clusters = new List<Cluster>();
            for(int i = 0; i < k; i++)
                if(!Centers[i].IsNaN()) // check for degenerate clusters
                    clusters.Add(new Cluster
                    {
                        Id = i + 1,
                        Center = Centers[i],
                        Members = objects[i].ToArray(),
                        Children = new Cluster[] { }
                    });

            // return single cluster with K children
            return new Cluster 
            { 
                Id = 0, 
                Center = X.Mean(VectorType.Row), 
                Children = clusters.ToArray() 
            };
        }
        /// <summary>Generates.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="k">The int to process.</param>
        /// <param name="metric">the metric.</param>
        /// <returns>An int[].</returns>
        public int[] Generate(Matrix x, int k, IDistance metric)
        {
            if (metric == null)
                metric = new EuclidianDistance();

            X = x;

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

                    // bounds?
                    if (min_index == -1)
                        min_index = 0;
                    assignments[j] = min_index;
                });

                // Update Step
                Matrix new_means = Matrix.Zeros(k, X.Cols);
                Vector sum = Vector.Zeros(k);

                // Part 1: Sum up assignments
                for (int j = 0; j < X.Rows; j++)
                {
                    int a = assignments[j];
                    new_means[a] += X[j, VectorType.Row];
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
        /// <summary>Generates.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples">The examples.</param>
        /// <param name="k">The int to process.</param>
        /// <param name="metric">(Optional) the metric.</param>
        /// <returns>An int[].</returns>
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
        /// <summary>Initializes the uniform.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="k">The int to process.</param>
        /// <returns>A Matrix.</returns>
        private Matrix InitializeUniform(Matrix X, int k)
        {
            int multiple = (int)System.Math.Floor((double)X.Rows / (double)k);

            var m = Matrix.Zeros(k, X.Cols);

            for (int i = 0; i < k; i++)
                for (int j = 0; j < X.Cols; j++)
                    m[i, j] = X[i * multiple, j];

            return m;

        }
        /// <summary>Initializes the random.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="k">The int to process.</param>
        /// <returns>A Matrix.</returns>
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
