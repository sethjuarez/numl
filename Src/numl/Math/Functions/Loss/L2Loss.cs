using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Math.LinearAlgebra;
using numl.Supervised;

namespace numl.Math.Functions.Loss
{
    public class L2Loss : ILossFunction
    {
        /// <summary>
        /// Computes the L2 loss delta between the two vectors.
        /// </summary>
        /// <param name="x">Predicted values.</param>
        /// <param name="y">Actual values.</param>
        /// <returns>Vector.</returns>
        public double Compute(Vector x, Vector y)
        {
            return Score.ComputeRMSE(y, x);
        }
    }
}
