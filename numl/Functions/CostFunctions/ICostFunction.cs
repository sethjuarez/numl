using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Functions
{
    /// <summary>
    /// Cost function interface
    /// </summary>
    public interface ICostFunction
    {
        /// <summary>
        /// Computes the cost of the current theta parameters against the known Y labels
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="X"></param>
        /// <param name="y"></param>
        /// <param name="lambda">Regaularisation constant</param>
        /// <param name="regularizationTerm">Regularization method to apply</param>
        /// <returns></returns>
        double ComputeCost(Vector theta, Matrix X, Vector y, double lambda, IRegularizer regularizationTerm);

        /// <summary>
        /// Computes the current gradient step direction towards the minima
        /// </summary>
        /// <param name="theta">Current theta parameters</param>
        /// <param name="X">Training set</param>
        /// <param name="y">Training labels</param>
        /// <param name="lambda">Regularisation constant</param>
        /// <param name="regularizationTerm">Regularization method to apply</param>
        /// <returns></returns>
        Vector ComputeGradient(Vector theta, Matrix X, Vector y, double lambda, IRegularizer regularizationTerm);
    }
}
