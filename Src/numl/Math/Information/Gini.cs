// file:	Math\Information\Gini.cs
//
// summary:	Implements the gini class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Information
{
    /// <summary>
    /// This class calculates the Gini Index of any given vector. It inherits from
    /// <see cref="Impurity"/> class which provides additional functionality.
    /// </summary>
    public class Gini : Impurity
    {
        /// <summary>Calculates Gini Index of x.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The list in question.</param>
        /// <returns>Impurity measure.</returns>
        public override double Calculate(Vector x)
        {
            if (x == null)
                throw new InvalidOperationException("x does not exist!");

            double length = x.Count();

            var px = (from i in x.Distinct()
                      let q = (from j in x
                               where j == i
                               select j).Count()
                      select q / length);

            var g = 1 - px.Select(d => d * d).Sum();

            return g;
        }
    }
}
