/*
 Copyright (c) 2011 Seth Juarez

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
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Supervised
{
    public class PerceptronGenerator : Generator
    {
        public bool Normalize { get; set; }

        public PerceptronGenerator()
        {
            Normalize = true;
        }

        public PerceptronGenerator(bool normalize)
        {
            Normalize = normalize;
        }

        public override IModel Generate(Matrix X, Vector Y)
        {
            Vector w = new DenseVector(X.ColumnCount);
            Vector a = (Vector)w.Clone();

            double wb = 0;
            double ab = 0;

            int n = 1;

            if (Normalize)
            {
                for (int i = 0; i < X.RowCount; i++)
                {
                    var norm = X.Row(i).Norm(2);
                    for (int j = 0; j < X.ColumnCount; j++)
                        X[i, j] = X[i, j] / norm;
                }
            }

            // repeat 10 times for *convergence*
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < X.RowCount; j++)
                {
                    var x = (Vector)X.Row(j);
                    var y = Y[j];

                    // perceptron update
                    if (y * (w.DotProduct(x) + wb) <= 0)
                    {
                        w = (Vector)w.Add(y * x);
                        wb += y;
                        a = (Vector)(a.Add(y * x)).Add(n);
                        ab += y * n;
                    }

                    n += 1;
                }
            }

            return new PerceptronModel
            {
                W = (Vector)w.Subtract(a.Divide(n)),
                B = wb - (ab / n),
                Normalized = Normalize
            };
        }
    }

    public class PerceptronModel : Model
    {
        public Vector W { get; set; }
        public double B { get; set; }
        public bool Normalized { get; set; }

        public override double Predict(Vector y)
        {
            if (Normalized)
                y = (Vector)(y / y.Norm(2));

            return W.DotProduct(y) + B;
        }
    }
}
