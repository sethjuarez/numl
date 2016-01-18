using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;

namespace numl.Supervised.SVM.Selection
{
    /// <summary>
    /// Interface for working set selection routines for Support Vector Machines.
    /// </summary>
    public interface ISelection
    {
        /// <summary>
        /// Gets or sets the standard regularization value C.
        /// <para>Lower C values will prevent overfitting.</para>
        /// </summary>
        double C { get; set; }

        /// <summary>
        /// Gets or sets the margin tolerance factor (default is 0.001).
        /// </summary>
        double Epsilon { get; set; }

        /// <summary>
        /// Gets or sets the starting bias value (Optional, default is 0).
        /// </summary>
        double Bias { get; set; }

        /// <summary>
        /// Gets or sets the precomputed Kernel matrix.
        /// </summary>
        Matrix K { get; set; }

        /// <summary>
        /// Gets or sets the training example labels in +1/-1 form.
        /// </summary>
        Vector Y { get; set; }

        /// <summary>
        /// Initializes the selection function.
        /// </summary>
        /// <param name="alpha">Alpha vector</param>
        /// <param name="gradient">Gradient vector.</param>
        void Initialize(Vector alpha, Vector gradient);

        /// <summary>
        /// Gets a new working set selection of i, j pair.
        /// </summary>
        /// <param name="i">Current working set pair i.</param>
        /// <param name="j">Current working set pair j.</param>
        /// <param name="gradient">Current Gradient vector.</param>
        /// <param name="alpha">Current alpha parameter vector.</param>
        /// <returns>New working pairs of i, j.  Returns </returns>
        Tuple<int, int> GetWorkingSet(int i, int j, Vector gradient, Vector alpha);
    }
}
