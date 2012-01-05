/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using numl.Math;

namespace numl.Supervised
{
    public class LinearRegressionModel : IGenerator
    {
        public IModel Generate(Matrix x, Vector y)
        {
            // normalize each each row
            for (int i = 0; i < x.Rows; i++)
                x[i, VectorType.Row] = x[i, VectorType.Row] / Vector.Norm(x[i, VectorType.Row]);

            // calculate W 
            // Could throw the following exceptions:
            // 1. SingularMatrixException, inverse becomes unstable, 
            //    This could happen if X.T * X is not full rank, although
            //    this should ALWAYS be symmetric positive definite
            //    Equivalent to Moore-Penrose pseudoinverse
            // 2. InvalidOperationException because of invalid Matrix to Vector conversion

            // this is dumb, I need to do a Cholesky factorization + backsolve
            // to ensure stability of operation
            var W = ((((x.T * x) ^ -1) * x.T) * y).ToVector();

            // bias term
            double B = y.Mean() - Vector.Dot(W, x.GetRows().Mean());

            // create predictor
            return new LinearRegressionPredictor
            {
                W = W,
                B = B
            };
        }
    }
}
