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

namespace numl.Supervised
{
    public class LinearRegressionModel : IModel
    {
        public LabeledDescription Description { get; set; }
        public Vector W { get; set; }
        public double B { get; set; }

        public double Predict(Vector y)
        {
            // calculate estimate using normalized example
            return W.Dot(y / y.Norm()) + B;
        }

        public object Predict(object o)
        {
            var label = Description.Label;
            var pred = Predict(Description.ToVector(o));
            var val = R.Convert(pred, label.Type);
            R.Set(o, label.Name, val);
            return o;
        }

        public T Predict<T>(T o)
        {
            return (T)Predict((object)o);
        }
    }
}
