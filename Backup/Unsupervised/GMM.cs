// file:	Unsupervised\GMM.cs
//
// summary:	Implements the gmm class
using System;
using numl.Math;
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;
using numl.Math.Metrics;

namespace numl.Unsupervised
{
    /// <summary>A gmm.</summary>
    public class GMM
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }
        /// <summary>Generates.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples">The examples.</param>
        /// <param name="k">The int to process.</param>
        public void Generate(IEnumerable<object> examples, int k)
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

            // Extract data
            Matrix X = Descriptor.ToMatrix(examples);

            // generate model
            Generate(X, k);
        }
        /// <summary>Generates.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="k">The int to process.</param>
        public void Generate(Matrix X, int k)
        {
            int n = X.Rows;
            int d = X.Cols;
            
            /***********************
             * initialize parameters
             ***********************/
            // convergence params
            var log_probability = 0d;
            var probability_difference = double.MaxValue;
            var mu_difference = double.MaxValue;

            // initialize centers with KMeans
            KMeans kmeans = new KMeans();
            var asgn = kmeans.Generate(X, k, new EuclidianDistance());

            // tentative centers
            var mu_k = kmeans.Centers;

            // initial covariances (stored as diag(cov) 1 of k)
            var sg_k = new Matrix(k, d);
            for (int i = 0; i < k; i++)
            {
                var indices = asgn.Select((a, b) => new Tuple<int, int>(a, b)).Where(t => t.Item1 == i).Select(t => t.Item2);
                var matrix = X.Slice(indices, VectorType.Row);
                sg_k[i] = matrix.CovarianceDiag();
            }

            // mixing coefficient
            var pi_k = asgn
                        .OrderBy(i => i)
                        .GroupBy(j => j)
                        .Select(g => (double)g.Count() / (double)asgn.Length)
                        .ToVector();

            int max_iter = 100;
            do
            {
                /***********************
                 * Expectation Step
                 ***********************/
                // responsibilty matrix: how much is gaussian k responsible for this point x
                var z_nk = new Matrix(n, k);
                for (int i = 0; i < n; i++)
                {
                    //  pi_j * N(x_n | mu_j, sigma_j)
                    for (int j = 0; j < k; j++)
                        z_nk[i, j] = pi_k[j] * Normal(X[i], mu_k[j], sg_k[j]);

                    var dn = z_nk[i].Sum();

                    if(dn == 0)
                        Console.WriteLine("Uh oh....");

                    z_nk[i].Each(z => z / dn);
                }

                /***********************
                 * Maximization Step
                 ***********************/
                var N_k = z_nk.Sum(VectorType.Row);

                var mu_k_new = new Matrix(mu_k.Rows, mu_k.Cols);
                for (int i = 0; i < k; i++)
                {
                    var sum = Vector.Zeros(d);
                    for (int j = 0; j < n; j++)
                        sum += z_nk[j, i] * X[j];
                    mu_k_new[i] = sum / N_k[i];
                }

                var sg_k_new = new Matrix(k, d);
                for (int i = 0; i < k; i++)
                {
                    var sum = Vector.Zeros(d);
                    for (int j = 0; j < n; j++)
                        sum += z_nk[j, i] * (X[j] - mu_k_new[i]).Each(s => s * s);
                    sg_k_new[i] = sum / N_k[i];
                }

                var pi_k_new = N_k / n;

                /***********************
                 * Convergence Check
                 ***********************/
                var new_log_prob = 0d;
                for (int i = 0; i < n; i++)
                {
                    var acc = 0d;
                    //  pi_j * N(x_n | mu_j, sigma_j)
                    for (int j = 0; j < k; j++)
                        acc += pi_k[j] * Normal(X[i], mu_k[j], sg_k[j]);

                    new_log_prob += System.Math.Log(acc, System.Math.E);
                }

                // log likelihood differences
                probability_difference = System.Math.Abs(log_probability - new_log_prob);
                Console.WriteLine("Log Likelihoods (Total Points: {0}, k={1}, d={2})\nO: {3}\nN: {4}\nDifference: {5}\n", n, k, d, log_probability, new_log_prob, probability_difference);
                log_probability = new_log_prob;


                // centers differences
                mu_difference = mu_k.GetRows()
                                .Zip(mu_k_new.GetRows(), (v1, v2) => new { V1 = v1, V2 = v2 })
                                .Sum(a => (a.V1 - a.V2).Norm());

                Console.WriteLine("Centers:\nO: {0}\nN: {1}\nDifference: {2}\n", mu_k, mu_k_new, mu_difference);
                mu_k = mu_k_new;

                // covariance differences
                double diff = sg_k.GetRows()
                            .Zip(sg_k_new.GetRows(), (v1, v2) => new { V1 = v1, V2 = v2 })
                            .Sum(a => (a.V1 - a.V2).Norm());

                Console.WriteLine("Covariance:\nO: {0}\nN: {1}\nDifference: {2}\n", sg_k, sg_k_new, diff);
                sg_k = sg_k_new;

                // mixing differences
                diff = (pi_k - pi_k_new).Each(s => System.Math.Abs(s)).Sum();
                Console.WriteLine("Mixing Coeffs:\nO: {0}\nN: {1}\nDifference: {2}\n", pi_k, pi_k_new, diff);
                pi_k = pi_k_new;

                Console.WriteLine("-------------------------------------------------------------");

            } while (probability_difference > .0000000001 && mu_difference > .0000000001 && --max_iter >= 0);
        }
        /// <summary>Compute probability according to multivariate Gaussian.</summary>
        /// <param name="x">Vector in question.</param>
        /// <param name="mu">Mean.</param>
        /// <param name="sigma">diag(covariance)</param>
        /// <returns>Probability.</returns>
        private double Normal(Vector x, Vector mu, Vector sigma)
        {
            // 1 / (2pi)^(2/D) where D = length of sigma
            var one_over_2pi = 1 / System.Math.Pow(2 * System.Math.PI, 2 / sigma.Length);

            // 1 / sqrt(det(sigma)) where det(sigma) is the product of the diagonals
            
            var one_over_det_sigma = System.Math.Sqrt(sigma.Aggregate(1d, (a, i) => a *= i));

            // -.5 (x-mu).T sigma^-1 (x-mu) I have taken some liberties ;)
            var exp = -0.5d * ((x - mu) * sigma.Each(d => 1 / d, true)).Dot(x - mu);

            // e^(exp)
            var e_exp = System.Math.Pow(System.Math.E, exp);
            
            var result = one_over_2pi * one_over_det_sigma * e_exp;

            return result;
        }
    }
}
