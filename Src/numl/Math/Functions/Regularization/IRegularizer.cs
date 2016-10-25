using numl.Math.LinearAlgebra;

namespace numl.Math.Functions.Regularization
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
        /// <param name="m">Number of training records</param>
        /// <param name="lambda">Regularization constant</param>
        /// <returns></returns>
        double Regularize(double j, Vector theta, int m, double lambda);

        /// <summary>
        /// Applies regularization to the current gradient
        /// </summary>
        /// <param name="gradient">Current gradient</param>
        /// <param name="theta">Current theta</param>
        /// <param name="m">Number of training records</param>
        /// <param name="lambda">Regularization constant</param>
        Vector Regularize(Vector gradient, Vector theta, int m, double lambda);
    }
}
