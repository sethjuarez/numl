using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;

namespace numl.Math.Functions
{
    /// <summary>
    /// Softmax function
    /// </summary>
    public class Softmax : IFunction
    {
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
            Vector d = this.Compute(x);
            return d * (1.0 - d);
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
    }
}
