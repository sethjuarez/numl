using numl.Math.LinearAlgebra;
using numl.Math.Functions.Regularization;
namespace numl.Math.Functions.Cost
{
    /// <summary>
    /// Cost function interface
    /// </summary>
    public interface ICostFunction
    {
        /// <summary>
        /// Gets or sets te input matrix.
        /// </summary>
        Matrix X { get; set; }

        /// <summary>
        /// Gets or sets the output for each row in X.
        /// </summary>
        Vector Y { get; set; }

        /// <summary>
        /// Gets or sets the weight decay (lambda) parameter.
        /// </summary>
        double Lambda { get; set; }

        /// <summary>
        /// Gets or sets the regularization method.
        /// </summary>
        IRegularizer Regularizer { get; set; }

        /// <summary>
        /// Initialization method for performing custom actions prior to being optimized.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Computes the cost of the current theta parameters against the known Y labels
        /// </summary>
        /// <returns></returns>
        double ComputeCost(Vector theta);

        /// <summary>
        /// Computes the current gradient step direction towards the minima
        /// </summary>
        /// <returns></returns>
        Vector ComputeGradient(Vector theta);
    }
}
