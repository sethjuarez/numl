using System;
using System.Linq;
using numl.Math.LinearAlgebra;

namespace numl.Math.Functions
{
    /// <summary>
    /// Softmax function
    /// </summary>
    public class Softmax : IFunction
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to 1.0.
        /// </summary>
        public double Maximum { get { return 1; } }

        /// <summary>
        /// Returns a softmax function vector from the supplied inputs.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Vector Compute(Vector x)
        {
            double max = x.Max();
            Vector softmax = x.Each(v => System.Math.Exp(v - max));

            double sum = softmax.Sum();

            softmax = softmax.Each(s => s / sum); 

            return softmax;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Compute(double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes the derivation of the softmax function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Vector Derivative(Vector x)
        {
            Vector d = Compute(x);
            return d * (1d - d);
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Derivative(double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the maximum value index of the computed softmax vector.
        /// </summary>
        /// <param name="x">Vector.</param>
        /// <returns>Double.</returns>
        public double Minimize(Vector x)
        {
            return Compute(x).MaxIndex();
        }
    }
}
