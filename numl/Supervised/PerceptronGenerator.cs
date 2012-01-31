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
using numl.Model;
using System.Collections.Generic;

namespace numl.Supervised
{
    public class PerceptronGenerator : IGenerator
    {
        public LabeledDescription Description { get; set; }
        public bool Normalize { get; set; }

        public PerceptronGenerator(bool normalize = true)
        {
            Normalize = normalize;
        }

        
        public IModel Generate(LabeledDescription description, IEnumerable<object> examples)
        {
            Description = description;
            var data = examples.ToExamples(Description);
            return Generate(data.Item1, data.Item2);
        }

        public IModel Generate(Matrix X, Vector Y)
        {
            Vector w = Vector.Zeros(X[0].Length);
            Vector a = w.Copy();

            double wb = 0;
            double ab = 0;

            int n = 1;

            if (Normalize)
                for (int j = 0; j < X.Rows; j++)
                    X[j] = X[j] / X[j].Norm();

            // repeat 10 times for *convergence*
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < X.Rows; j++)
                {
                    Vector x = X[j];
                    double y = Y[j];

                    // perceptron update
                    if (y * (Vector.Dot(w, x) + wb) <= 0)
                    {
                        w += (y * x);
                        wb += y;
                        a += (y * x) * n;
                        ab += y * n;
                    }

                    n += 1;
                }
            }

            return new PerceptronModel
            {
                W = w - (a / n),
                B = wb - (ab / n),
                Normalized = Normalize
            };
        }
    }
}
