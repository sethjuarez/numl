using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;

namespace numl.Unsupervised
{
    public class PCA
    {
        public void Generate(Matrix m)
        {
            m.Center(VectorType.Column);
            var cov = m.Covariance();
            var eigs = cov.Eigs();
            var eigenvalues = eigs.Item1;
            var eigenvector = eigs.Item2;

        }
    }
}
