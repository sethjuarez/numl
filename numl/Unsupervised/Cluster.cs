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
using System.Linq;
using numl.Math;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Unsupervised
{
    public class Cluster
    {
        internal int Id { get; set; }
        internal IEnumerable<Vector> Points { get; set; }
        public IEnumerable<object> Members { get; set; }
        public Cluster[] Children { get; set; }

        public Cluster()
        {
        }

        public Cluster(int id, Cluster left, Cluster right )
        {
            Id = id;
            Children = new Cluster[] { left, right };
            Points = left.Points.Concat(right.Points);
            // maybe only need item at leaves
            Members = left.Members.Concat(right.Members).ToArray();
        }

        public Cluster(int id, IEnumerable<Cluster> children)
        {
            Id = id;
            Children = children.ToArray();
        }
    }
}
