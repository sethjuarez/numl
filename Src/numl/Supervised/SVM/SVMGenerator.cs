using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.SVM
{
    /// <summary>A Support Vector Machine (SVM) generator.</summary>
    public class SVMGenerator : Generator
    {
        /// <summary>
        /// Gets or sets the standard regularization value C.
        /// <para>Lower C values will prevent overfitting.</para>
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Gets or sets the margin tolerance factor (default is 0.001).
        /// </summary>
        public double Epsilon { get; set; }

        /// <summary>
        /// Gets or sets the starting bias value (Optional, default is 0).
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of passes to attempt without changes before converging.
        /// </summary>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the Kernel function to use for computing the similarity of support vectors.
        /// </summary>
        public IKernel KernelFunction { get; set; }

        /// <summary>
        /// Gets or sets the Working Set Selection function for selecting new i, j support vectors.
        /// </summary>
        public numl.Supervised.SVM.Selection.ISelection SelectionFunction { get; set; }

        /// <summary>
        /// Initialises a SVMGenerator object
        /// </summary>
        public SVMGenerator()
        {
            this.Bias = 0d;
            this.C = 1d;
            this.Epsilon = 0.001;
            this.MaxIterations = 10;
            this.KernelFunction = new LinearKernel();
            this.NormalizeFeatures = true;

            if (this.SelectionFunction == null)
                this.SelectionFunction = new numl.Supervised.SVM.Selection.WorkingSetSelection3();
        }

        /// <summary>Generates a SVM model based on a set of examples.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            this.Preprocess(X);

            // expect truth = 1 and false = -1
            y = y.ToBinary(k => k == 1d, falseValue: -1.0);

            // initialise variables
            int m = X.Rows, n = X.Cols, i = -1, j = -1, changes = 0, iterations = 0;
            double lagLow = 0.0, lagHigh = 0.0, cost = 0.0, tempAI = 0d, tempAJ = 0d;

            Vector gradient = Vector.Zeros(m), alpha = Vector.Zeros(m);

            // precompute kernal matrix (using similarity function)
            Matrix K = this.KernelFunction.Compute(X);

            // synchronise SVM parameters with working set selection function.
            this.SelectionFunction.Bias = this.Bias; this.SelectionFunction.C = this.C; this.SelectionFunction.Epsilon = this.Epsilon;
            this.SelectionFunction.K = K; this.SelectionFunction.Y = y;

            bool finalise = false;

            this.SelectionFunction.Initialize(alpha, gradient);

            while (finalise == false && iterations < this.MaxIterations)
            {
                changes = 0;

                #region Training

                for (int p = 0; p < m; p++)
                {
                    // get new working set selection using heuristic function
                    Tuple<int, int> newPair = this.SelectionFunction.GetWorkingSet(i, j, gradient, alpha);

                    // check for valid i, j pairs
                    if (newPair.Item1 >= 0 && newPair.Item2 >= 0 && newPair.Item1 != newPair.Item2)
                    {
                        i = newPair.Item1; j = newPair.Item2;
                        // compute new gradients
                        gradient[i] = Bias + (alpha * y * K[i, VectorType.Col]).Sum() - y[i];

                        if ((y[i] * gradient[i] < -this.Epsilon && alpha[i] < this.C) || (y[i] * gradient[i] > this.Epsilon && alpha[i] > 0))
                        {
                            gradient[j] = Bias + (alpha * y * K[j, VectorType.Col]).Sum() - y[j];

                            // store temp working copies of alpha from both pairs (i, j)
                            tempAI = alpha[i]; tempAJ = alpha[j];

                            // update lower and upper bounds of lagrange multipliers
                            if (y[i] == y[j])
                            {
                                // pairs are same class don't apply large margin
                                lagLow = System.Math.Max(0.0, alpha[j] + alpha[i] - this.C);
                                lagHigh = System.Math.Min(this.C, alpha[j] + alpha[i]);
                            }
                            else
                            {
                                // pairs are not same class, apply large margin
                                lagLow = System.Math.Max(0.0, alpha[j] - alpha[i]);
                                lagHigh = System.Math.Min(this.C, this.C + alpha[j] - alpha[i]);
                            }

                            // if lagrange constraints are not diverse then get new working set
                            if (lagLow == lagHigh) continue;

                            // compute cost and if it's greater than 0 skip
                            // cost should optimise large margin where fit line intercepts <= 0
                            cost = 2.0 * K[i, j] - K[i, i] - K[j, j];
                            if (cost >= 0.0) continue;
                            else
                            {
                                // update alpha of (j) w.r.t to the relative cost difference of the i-th and j-th gradient
                                alpha[j] = alpha[j] - (y[j] * (gradient[i] - gradient[j])) / cost;

                                // clip alpha with lagrange multipliers
                                alpha[j] = System.Math.Min(lagHigh, alpha[j]);
                                alpha[j] = System.Math.Max(lagLow, alpha[j]);

                                // check alpha tolerance factor
                                if (System.Math.Abs(alpha[j] - tempAJ) < this.Epsilon)
                                {
                                    // we're optimising large margins so skip small ones
                                    alpha[j] = tempAJ; continue;
                                }

                                // update alpha of i if we have a large margin w.r.t to alpha (j)
                                alpha[i] = alpha[i] + y[i] * y[j] * (tempAJ - alpha[j]);

                                // precompute i, j into feasible region for Bias
                                double yBeta = (alpha[i] - tempAI) * K[i, j] - y[j] * (alpha[j] - tempAJ);
                                // store temp beta with gradient for i, j pairs
                                double beta_i = this.Bias - gradient[i] - y[i] * yBeta * K[i, j];
                                double beta_j = this.Bias - gradient[j] - y[i] * yBeta * K[j, j];

                                // update new bias with constrained alpha limits (0 < alpha < C)
                                if (0.0 < alpha[i] && alpha[i] < this.C) this.Bias = beta_i;
                                else if (0.0 < alpha[j] && alpha[j] < this.C) this.Bias = beta_j;
                                else this.Bias = (beta_i + beta_j) / 2.0;

                                changes++;
                            }
                        }
                    }
                    else if (newPair.Item1 == -1 || newPair.Item2 == -1)
                    {
                        // unable to find suitable sub problem (j) to optimise
                        finalise = true;
                        break;
                    }
                }

                if (changes == 0) iterations++;
                else iterations = 0;

                #endregion
            }

            // get only supporting parameters where alpha is positive
            // i.e. because 0 < alpha < large margin
            int[] fitness = (alpha > 0d).ToArray();

            // return initialised model
            return new SVMModel()
            {
                Descriptor = this.Descriptor,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Theta = ((alpha * y) * X).ToVector(),
                Alpha = alpha.Slice(fitness),
                Bias = this.Bias,
                X = X.Slice(fitness, VectorType.Row),
                Y = y.Slice(fitness),
                KernelFunction = this.KernelFunction
            };
        }
    }
}
