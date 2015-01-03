// file:	Unsupervised\PCA.cs
//
// summary:	Implements the pca class
using System;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    /// <summary>A pca.</summary>
    public class PCA
    {
        /// <summary>Gets or sets the eigenvalues.</summary>
        /// <value>The eigenvalues.</value>
        public Vector Eigenvalues { get; private set; }
        /// <summary>Gets or sets the eigenvectors.</summary>
        /// <value>The eigenvectors.</value>
        public Matrix Eigenvectors { get; private set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        public Matrix X { get; private set; }
        /// <summary>Gets or sets the reduced.</summary>
        /// <value>The reduced.</value>
        public Matrix Reduced { get; private set; }
        /// <summary>Generates.</summary>
        /// <param name="matrix">The matrix.</param>
        public void Generate(Matrix matrix)
        {
            // generate centered matrix
            // (using a copy since centering is in place)
            X = matrix
                    .Copy()
                    .Center(VectorType.Col);

            // compute eigen-decomposition
            // of covariance matrix
            var eigs = X.Covariance().Eigs();
            Eigenvalues = eigs.Item1;
            Eigenvectors = eigs.Item2;
        }
        /// <summary>Generates.</summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="examples">The examples.</param>
        public void Generate(Descriptor descriptor, IEnumerable<object> examples)
        {
            // generate data matrix
            var x = descriptor.Convert(examples).ToMatrix();
            Generate(x);
        }
        /// <summary>Reduces.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="maxdim">The maxdim.</param>
        /// <returns>A Matrix.</returns>
        public Matrix Reduce(int maxdim)
        {
            if(maxdim < 1) throw new InvalidOperationException("Cannot reduce to less than 1 dimension!");
            if (X == null || Eigenvalues == null || Eigenvectors == null)
                throw new InvalidOperationException("Cannot reduce until pca data has been generated");

            // get columns in reverse order
            // and stuff into matrix
            Matrix reduc = Eigenvectors.GetCols()
                                .Take(maxdim)
                                .ToMatrix();

            Reduced = reduc * X.T;

            return Reduced;
        }
    }
}
