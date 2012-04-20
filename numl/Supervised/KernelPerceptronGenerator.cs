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
using System.Collections.Generic;
using numl.Model;

namespace numl.Supervised
{
    public class KernelPerceptronGenerator : Generator
    {
        public KernelType Type { get; set; }
        public double P { get; set; }

        public KernelPerceptronGenerator(double kernelParam = 2, KernelType type = KernelType.Polynomial)
        {
            Type = type;
            P = kernelParam;
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            int N = y.Length;
            Vector a = Vector.Zeros(N);
            Matrix K = GetKernel(x);

            int n = 1;

            // hopefully enough to 
            // converge right? ;)
            // need to be smarter about
            // storing SPD kernels...
            bool found_error = true;
            while (n < 500 && found_error)
            {
                found_error = false;
                for (int i = 0; i < N; i++)
                {
                    if (y[i] * a.Dot(K[i, VectorType.Row]) <= 0)
                    {
                        a[i] += y[i];
                        found_error = true;
                    }
                }

                n++;
            }

            // anything that *matters*
            // i.e. support vectors
            var indices = a.Indices(d => d != 0);

            // slice up examples to contain
            // only support vectors
            return new KernelPerceptronModel
            {
                A = a.Slice(indices),
                Y = y.Slice(indices),
                X = x.Slice(indices, VectorType.Row),
                Type = Type,
                Param = P
            };
        }

        public Matrix GetKernel(Matrix X)
        {
            Matrix K = Matrix.Zeros(1);
            switch (Type)
            {
                case KernelType.Polynomial:
                    K = X.PolyKernel(P);
                    break;
                case KernelType.RBF:
                    K = X.RBFKernel(P);
                    break;
            }

            return K;
        }
    }
}
