using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Utils;

namespace numl.Supervised.SVM.Selection
{
    /// <summary>
    /// Implements Working Set Selection 3 which uses second order information for selecting new pairs.
    /// </summary>
    public class WorkingSetSelection3 : ISelection
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
        /// Gets or sets the precomputed Kernel matrix.
        /// </summary>
        public Matrix K { get; set; }

        /// <summary>
        /// Gets or sets the training example labels in +1/-1 form.
        /// </summary>
        public Vector Y { get; set; }

        /// <summary>
        /// Initializes the selection function.
        /// </summary>
        /// <param name="alpha">Alpha vector</param>
        /// <param name="gradient">Gradient vector.</param>
        public void Initialize(Vector alpha, Vector gradient)
        {
            alpha.Each((d) => 0, false);
            gradient.Each((d) => -1, false);
        }

        /// <summary>
        /// Gets a new working set selection of i, j pair.
        /// </summary>
        /// <param name="i">Current working set pair i.</param>
        /// <param name="j">Current working set pair j.</param>
        /// <param name="gradient">Current Gradient vector.</param>
        /// <param name="alpha">Current alpha parameter vector.</param>
        /// <returns>New working pairs of i, j.  Returns </returns>
        public Tuple<int, int> GetWorkingSet(int i, int j, Vector gradient, Vector alpha)
        {
            int m = this.Y.Length, ij = -1, jj = -1;
            double maxGrad = double.NegativeInfinity, minGrad = double.PositiveInfinity,
                minObj = double.PositiveInfinity, a = 0.0, b = 0.0, tempGrad = 0.0, tempObj;

            double tau = System.Math.Pow(this.Epsilon, 2.0);

            // choose i
            for (int k = 0; k < m; k++)
            {
                // check for analytical constraints:
                if ((this.Y[k] >= 1.0 && alpha[k] < this.C) || (this.Y[k] <= 0.0 && alpha[k] > 0.0))
                {
                    tempGrad = -this.Y[k] * gradient[k];
                    if (tempGrad >= maxGrad)
                    {
                        // store new best fit
                        ij = k;
                        maxGrad = tempGrad;
                    }
                }
            }
            // choose j that best optimises i
            for (int k = 0; k < m; k++)
            {
                // check for analytical constraints:
                // (this.Y[k] > A > 0 or this.Y[k] < 0 < A < C)
                if ((this.Y[k] >= 1.0 && alpha[k] > 0.0) || (this.Y[k] <= 0.0 && alpha[k] < this.C))
                {
                    b = maxGrad + this.Y[k] * gradient[k];

                    if (-this.Y[k] * gradient[k] <= minGrad)
                        minGrad = -this.Y[k] * gradient[k];
                    if (b > 0.0)
                    {
                        // compute kernel sub-pair
                        a = K[ij, ij] + K[k, k] - 2 * this.Y[ij] * this.Y[k] * K[ij, k];
                        if (a <= 0)
                            a = tau;

                        tempObj = -(b * b) / a;
                        if (tempObj <= minObj)
                        {
                            // store new best fit and it's cost
                            jj = k;
                            minObj = tempObj;
                        }
                    }
                }
            }
            // check tolerance of computed gradient range
            if (maxGrad - minGrad < this.Epsilon)
            {
                return new Tuple<int, int>(-1, -1);
            }
            if (jj == -1 || ij == jj)
            {
                jj = (int)Sampling.GetUniform(-1, m).Clip(0, m - 1);
            }

            return new Tuple<int, int>(ij, jj);
        }
    }
}
