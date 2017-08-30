using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;

namespace numl.Math.Discretization
{
    /// <summary>
    /// Discretizer abstract class.
    /// </summary>
    public abstract class Discretizer : IDiscretizer
    {
        /// <summary>
        /// Gets or sets the Epsilon regularization parameter, used to avoid mathematical divide-by-zero errors.
        /// </summary>
        public double Epsilon { get; set; } = Defaults.Epsilon;

        /// <summary>
        /// Performs any preconditioning steps prior to discretizing values.
        /// </summary>
        /// <param name="rows">Matrix.</param>
        /// <param name="summary">Feature properties from the original set.</param>
        public virtual void Initialize(Matrix rows, Summary summary) { }

        /// <summary>
        /// Discretizes a Vector into a single real value.
        /// </summary>
        /// <param name="row">Vector to process.</param>
        /// <param name="summary">Feature properties from the original set.</param>
        /// <returns>Double.</returns>
        public abstract double Discretize(Vector row, Summary summary);
    }
}
