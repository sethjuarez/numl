using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Math.LinearAlgebra;

using numl.Supervised;

namespace numl.Math.Functions.Loss
{
    public class CrossEntropyLoss : ILossFunction
    {
        /// <summary>
        /// Computes the Cross Entropy loss delta between the two vectors.
        /// </summary>
        /// <param name="x">Predicted values.</param>
        /// <param name="y">Actual values.</param>
        /// <returns>double.</returns>
        public double Compute(Vector x, Vector y)
        {
            return Score.ComputeCrossEntropy(x, y);
        }
    }
}
