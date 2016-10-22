// file:	Supervised\Perceptron\PerceptronModel.cs
//
// summary:	Implements the perceptron model class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Perceptron
{
    /// <summary>A data Model for the perceptron.</summary>
    public class PerceptronModel : Model
    {
        /// <summary>Gets or sets the w.</summary>
        /// <value>The w.</value>
        public Vector W { get; set; }
        /// <summary>Gets or sets the b.</summary>
        /// <value>The b.</value>
        public double B { get; set; }
        /// <summary>Gets or sets a value indicating whether the normalized.</summary>
        /// <value>true if normalized, false if not.</value>
        public bool Normalized { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            this.Preprocess(y);

            if (Normalized)
                y = y / y.Norm();

            return W.Dot(y) + B;
        }
    }
}
