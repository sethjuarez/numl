// file:	Math\Functions\IFunction.cs
//
// summary:	Declares the IFunction interface
using numl.Math.LinearAlgebra;

namespace numl.Math.Functions
{
    /// <summary>Interface for function.</summary>
    public interface IFunction
    {
        /// <summary>
        /// Returns the minimum value from the function curve.
        /// </summary>
        double Minimum { get; }
        
        /// <summary>
        /// Returns the maximum value from the function curve.
        /// </summary>
        double Maximum { get; }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The input to process.</param>
        /// <returns>Double.</returns>
        double Compute(double x);

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The input to the function.</param>
        /// <param name="y">Precomputed output of the function.</param>
        /// <returns>Double.</returns>
        double Derivative(double x, double y);

        /// <summary>
        /// Computes and condenses the given x coordinate.
        /// </summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>Double.</returns>
        double Minimize(Vector x);

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        Vector Compute(Vector x);

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The input to process.</param>
        /// <param name="y">Precomputed output of the function.</param>
        /// <returns>Vector.</returns>
        Vector Derivative(Vector x, Vector y);
    }
}
