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
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace numl.Model
{
    public class LabeledDescription : Description
    {
        public Property Label { get; set; }

        public override Property this[int i]
        {
            get
            {
                if (i > -1 && i < Features.Length)
                    return Features[i];
                else if (i == Features.Length)
                    return Label;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public override int Length
        {
            get
            {
                // last element is label
                return base.Length + 1;
            }
        }

        public override bool Verify()
        {
            if (Label == null)
                return false;

            if (Label is StringProperty)
            {
                StringProperty p = (StringProperty)Label;
                if (p.Dictionary == null || p.Dictionary.Length == 0)
                    return false;
            }

            return base.Verify();
        }

        public Tuple<Matrix, Vector> ToExamples(IEnumerable<object> collection)
        {
            // number of examples
            int n = collection.Count();
            Vector y = new Vector(n);

            double[][] matrix = new double[n][];

            int i = -1;
            foreach (object o in collection)
            {
                matrix[++i] = CreateFeatureArray(o);
                y[i] = Label.Convert(R.Get(o, Label.Name));
            }

            return new Tuple<Matrix, Vector>(new Matrix(matrix), y);
        }

        public Tuple<Matrix, Vector> ToExamples(IEnumerable collection)
        {
            List<double[]> matrix = new List<double[]>();
            List<double> y = new List<double>();
            foreach (object o in collection)
            {
                matrix.Add(CreateFeatureArray(o));
                y.Add(Label.ToArray(o)[0]);
            }

            return new Tuple<Matrix,Vector>(new Matrix(matrix.ToArray()), new Vector(y.ToArray()));
        }


    }
}
