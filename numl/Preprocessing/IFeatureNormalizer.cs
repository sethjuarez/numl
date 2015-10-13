using numl.Features;
using numl.Math.LinearAlgebra;

namespace numl.Preprocessing
{
    /// <summary>
    /// Interface for feature normalizer
    /// </summary>
    public interface IFeatureNormalizer
    {
        /// <summary>
        /// Vector method that normalizes the data, such as performing feature scaling.
        /// </summary>
        /// <param name="row">Single record to normalize</param>
        /// <param name="properties">Feature properties based on the original set</param>
        /// <returns></returns>
        Vector Normalize(Vector row, FeatureProperties properties);
    }
}