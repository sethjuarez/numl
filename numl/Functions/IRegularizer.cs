using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Functions
{
    /// <summary>
    /// Regularization function
    /// </summary>
    public interface IRegularizer
    {
        /// <summary>
        /// Applies regularization to the current cost
        /// </summary>
        /// <param name="j">Current cost</param>
        /// <param name="theta">Current theta</param>
        /// <param name="m">Training records</param>
        /// <param name="lambda">Regularization constant</param>
        /// <returns></returns>
        double Regularize(double j, Vector theta, int m, double lambda);

        /// <summary>
        /// Applies regularization to the current gradient
        /// </summary>
        /// <param name="theta">Current theta</param>
        /// <param name="gradient">Current gradient</param>
        /// <param name="m">Training records</param>
        /// <param name="lambda">Regularization constant</param>
        Vector Regularize(Vector theta, Vector gradient, int m, double lambda);
    }
}
