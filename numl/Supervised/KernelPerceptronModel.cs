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
using System.Xml.Serialization;
using numl.Model;

namespace numl.Supervised
{
    [XmlRoot("KernelPerceptron")]
    public class KernelPerceptronModel : Model
    {
        public KernelType Type { get; set; }
        public double Param { get; set; }
        public Matrix X { get; set; }
        public Vector Y { get; set; }
        public Vector A { get; set; }

        public override double Predict(Vector y)
        {
            var K = GetKernel(y);
            double v = 0;
            for (int i = 0; i < A.Length; i++)
                v += A[i] * Y[i] * K[i];

            return v;
        }

        private Vector GetKernel(Vector x)
        {
            Vector K = Vector.Zeros(1);
            switch (Type)
            {
                case KernelType.Polynomial:
                    K = X.PolyKernel(x, Param);
                    break;
                case KernelType.RBF:
                    K = X.RBFKernel(x, Param);
                    break;
            }

            return K;
        }
    }
}
