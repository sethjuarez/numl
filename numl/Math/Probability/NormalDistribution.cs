using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Math.Probability
{
    public class NormalDistribution
    {
        public Vector Mu { get; set; }
        public Matrix Sigma { get; set; }

        public void Estimate(Matrix X, VectorType type = VectorType.Row)
        {
            int n = type == VectorType.Row ? X.Rows : X.Cols;
            int s = type == VectorType.Row ? X.Cols : X.Rows;
            Mu = X.Sum(type) / n;
            Sigma = Matrix.Zeros(s);
            
            for (int i = 0; i < n; i++)
            {
                var x = X[i, type] - Mu;
                Sigma += x.Outer(x);
            }

            Sigma *= (1d / (n - 1d));
        }

        public double Compute(Vector x)
        {
            return 0;
        }
    }
}
