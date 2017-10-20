using System;
using System.Linq;
using numl.Math.LinearAlgebra;

namespace numl.Math.Functions
{
    /// <summary>
    /// Softmax function
    /// </summary>
    public class Softmax : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public override double Minimum => 0d;

        /// <summary>
        /// Returns the maximum value from the function curve, equal to 1.0.
        /// </summary>
        public override double Maximum => 1d;

        /// <summary>
        /// Returns a softmax function vector from the supplied inputs.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override Vector Compute(Vector x)
        {
            double max = x.Max();
            Vector softmax = Vector.Exp(x - max);

            double sum = softmax.Sum();

            softmax = (softmax / sum);

            return softmax;
        }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The value to process.</param>
        /// <returns>A double.</returns>
        public override double Compute(double x)
        {
            return exp(x);
        }

        /// <summary>Computes the derivation of the softmax function.</summary>
        /// <param name="x">The input to the function.</param>
        /// <param name="y">Precomputed softmax output.</param>
        /// <returns>Double.</returns>
        public override Vector Derivative(Vector x, Vector y)
        {
            var Y = y.ToMatrix();
            var J = Y.T * Y;
            var R = (Y.T * (1.0 - Y));
            var I = Matrix.Identity(J.Rows);
            var D = R.Each((v, i, j) => (i == j) ? v * I[i, j] : J[i, j]);
            return D.Sum(VectorType.Row);
        }

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The input to the function.</param>
        /// <param name="y">Precomputed softmax output.</param>
        /// <returns>Double.</returns>
        public override double Derivative(double x, double y)
        {
            return y - (1.0 - y);
        }

        /// <summary>
        /// Returns the maximum value index of the computed softmax vector.
        /// </summary>
        /// <param name="x">Vector.</param>
        /// <returns>Double.</returns>
        public override double Minimize(Vector x)
        {
            return Compute(x).MaxIndex();
        }
    }
}
