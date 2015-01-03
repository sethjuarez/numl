// file:	Supervised\Perceptron\PerceptronGenerator.cs
//
// summary:	Implements the perceptron generator class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Perceptron
{
    /// <summary>A perceptron generator.</summary>
    public class PerceptronGenerator : Generator
    {
        /// <summary>Gets or sets a value indicating whether the normalize.</summary>
        /// <value>true if normalize, false if not.</value>
        public bool Normalize { get; set; }

        /// <summary>Default constructor.</summary>
        public PerceptronGenerator()
        {
            Normalize = true;
        }
        /// <summary>Constructor.</summary>
        /// <param name="normalize">true to normalize.</param>
        public PerceptronGenerator(bool normalize)
        {
            Normalize = normalize;
        }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="Y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix X, Vector Y)
        {
            Vector w = Vector.Zeros(X.Cols);
            Vector a = Vector.Zeros(X.Cols);

            double wb = 0;
            double ab = 0;

            int n = 1;

            if (Normalize)
                X.Normalize(VectorType.Row);

            // repeat 10 times for *convergence*
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < X.Rows; j++)
                {
                    var x = X[j];
                    var y = Y[j];

                    // perceptron update
                    if (y * (w.Dot(x) + wb) <= 0)
                    {
                        w = w + y * x;
                        wb += y;
                        a = (a + y * x) + n;
                        ab += y * n;
                    }

                    n += 1;
                }
            }

            return new PerceptronModel 
            { 
                W = w - (a / n), 
                B = wb - (ab / n), 
                Normalized = Normalize, 
                Descriptor = Descriptor 
            };
        }
    }
}