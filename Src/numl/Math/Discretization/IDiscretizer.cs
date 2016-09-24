using numl.Math.LinearAlgebra;

namespace numl.Math.Discretization
{
    /// <summary>
    /// IDiscretizer interface.
    /// </summary>
    public interface IDiscretizer
    {
        /// <summary>
        /// Performs any preconditioning steps prior to discretizing values.
        /// </summary>
        /// <param name="rows">Matrix.</param>
        /// <param name="summary">Feature properties from the original set.</param>
        void Initialize(Matrix rows, Summary summary);

        /// <summary>
        /// Discretizes a Vector into a single real value.
        /// </summary>
        /// <param name="row">Vector to process.</param>
        /// <param name="summary">Feature properties from the original set.</param>
        /// <returns>Double.</returns>
        double Discretize(Vector row, Summary summary);
    }
}
