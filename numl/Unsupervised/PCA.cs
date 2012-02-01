using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;

namespace numl.Unsupervised
{
    public class PCA
    {
        public void Generate(Matrix matrix)
        {
            // generate centered covariance matrix
            // (using a copy since centering is in place)
            var cov = matrix
                        .Copy()
                        .Center(VectorType.Column)
                        .Covariance();
            
            // compute eigen-decomposition
            var eigs = cov.Eigs();
            var D = eigs.Item1;
            var V = eigs.Item2;

        }
    }
}
