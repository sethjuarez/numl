using System;
using numl.Math;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    public class GMM
    {
        public Description Description { get; set; }

        public void Generate(IEnumerable<object> examples, int k)
        {
            #region Sanity Checks
            if (examples == null)
                throw new InvalidOperationException("Cannot generate a model will no data!");

            if (k < 2)
                throw new InvalidOperationException("Can only cluter with k > 1");

            if (Description == null)
                throw new InvalidOperationException("Invalid Description!");

            int count = examples.Count();
            if (k >= count)
                throw new InvalidOperationException(
                    string.Format("Cannot cluster {0} items {1} different ways!", count, k));
            #endregion

            // Extract data
            Matrix X = examples.ToMatrix(Description);

            // generate model
            Generate(X, k);
        }

        public void Generate(Matrix X, int k)
        {
            int n = X.Rows;
            int d = X.Cols;
            // initialize centers with KMeans
            KMeans kmeans = new KMeans(Description);
            var data = kmeans.Generate(X, k);
            var asgn = data.Item2;
            /***********************
             * initialize parameters
             ***********************/
            // tentative centers
            var mu_k = data.Item1;
            // initial covariances (stored as diag(cov) 1 of k)
            var sg_k = new Matrix(d, k);
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

            /***********************
             * Expectation Step
             ***********************/
            // responsibilty matrix: how much is gaussian k responsible for this point x
            var z_nk = new Matrix(n, k);
            for (int i = 0; i < n; i++)
            {
                //  pi_j * N(x_n | mu_j, sigma_j
                for (int j = 0; j < k; j++)
                    z_nk[i, j] = pi_k[j] * Normal(X[i], mu_k[j], sg_k[j]);

                var denominator = z_nk[i].Sum();
                z_nk[i].Each(z => z / denominator);
            }


            /***********************
             * Maximization Step
             ***********************/
        }

        /// <summary>
        /// Compute probability according to multivariate Gaussian
        /// </summary>
        /// <param name="x">Vector in question</param>
        /// <param name="mu">Mean</param>
        /// <param name="sigma">diag(covariance)</param>
        /// <returns>Probability</returns>
        private double Normal(Vector x, Vector mu, Vector sigma)
        {
            // 1 / (2pi)^(2/D) where D = length of sigma
            var one_over_2pi = 1 / System.Math.Pow(2 * System.Math.PI, 2d / sigma.Length);

            // 1 / sqrt(det(sigma)) where det(sigma) is the product of the diagonals
            var one_over_det_sigma = System.Math.Sqrt(sigma.Aggregate(1d, (a, i) => a *= i));

            // -.5 (x-mu).T sigma^-1 (x-mu) I have taken some liberties ;)
            var exp = -0.5d * ((x - mu) * sigma.Each(d => 1 / d, true)).Dot(x - mu);

            // e^(exp)
            var e_exp = System.Math.Pow(System.Math.E, exp);

            return one_over_2pi * one_over_det_sigma * e_exp;
        }
    }
}
