using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Math.LinearAlgebra;

namespace numl.Math.Functions.Loss
{
    /// <summary>
    /// Delta loss function interface.
    /// </summary>
    public interface ILossFunction
    {
        /// <summary>
        /// Computes the delta between the two vectors.
        /// </summary>
        /// <param name="x">Predicted values.</param>
        /// <param name="y">Actual values.</param>
        /// <returns>double.</returns>
        double Compute(Vector x, Vector y);
    }
}
