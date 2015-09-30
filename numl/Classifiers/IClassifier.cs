using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Classification
{
    /// <summary>
    /// Interface for Classifiers
    /// </summary>
    public interface IClassifier
    {
        /// <summary>
        /// Calculates the raw probability of a positive value from the underlying learned Classifier model.
        /// </summary>
        /// <param name="x">A single test record.</param>
        /// <returns></returns>
        double PredictRaw(Math.LinearAlgebra.Vector x);
    }
}
