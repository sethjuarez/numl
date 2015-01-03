// file:	Supervised\Regression\LinearRegression.cs
//
// summary:	Implements the linear regression class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Regression
{
    /// <summary>A linear regression generator.</summary>
    public class LinearRegressionGenerator : Generator
    {
        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="NotImplementedException">Thrown when the requested operation is unimplemented.</exception>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            //x.NormalizeRows(2);

            throw new NotImplementedException();
        }
    }
}
