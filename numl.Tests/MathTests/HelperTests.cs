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
using NUnit.Framework;
using System.Collections.Generic;
using numl.Math;
using numl.Math.Information;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Tests.Math
{
    [TestFixture]
    public class HelperTests
    {
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 1.2d, 2.2, 2.1, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 1.2d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new double[] { })]
        public void Test_Enumerable_Slicing_With_Where_LessThanEqual(IEnumerable<double> source, double pivot, IEnumerable<double> truth)
        {
            var slice = Helpers.Slice(source, d => d <= pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 5.2d, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 2.2d, 2.1, 5.2, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        public void Test_Enumerable_Slicing_With_Where_GreaterThan(IEnumerable<double> source, double pivot, IEnumerable<double> truth)
        {
            var slice = Helpers.Slice(source, d => d > pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 0, 1, 2, 4 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 0, 4 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new int[] { })]
        public void Test_Enumerable_Indices_With_Where_LessThanEqual(IEnumerable<double> source, double pivot, IEnumerable<int> truth)
        {
            var slice = Helpers.Indices(source, d => d <= pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 3, 5, 6 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 1, 2, 3, 5, 6 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        public void Test_Enumerable_Indices_With_Where_GreaterThan(IEnumerable<double> source, double pivot, IEnumerable<int> truth)
        {
            var slice = Helpers.Indices(source, d => d > pivot);
            Assert.AreEqual(truth, slice);
        }


        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 2, 3, 4 }, new[] { 2.1d, 5.2, 0.2, })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 4, 2, 4, 2 }, new[] { 2.1d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 0, 6, 4, 3, 2, 5, 1 }, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 43, 9, 12 }, new double[] { })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new int[] { }, new double[] { })]
        public void Test_Enumerable_Slicing_With_Indices(IEnumerable<double> source, IEnumerable<int> indices, IEnumerable<double> truth)
        {
            var slice = Helpers.Slice(source, indices).ToArray();
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 2, 3, 4 }, new[] { 2.1d, 5.2, 0.2, })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 4, 2, 4, 2 }, new[] { 2.1d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 0, 6, 4, 3, 2, 5, 1 }, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        public void Test_Vector_Slicing_With_Indices(IEnumerable<double> source, IEnumerable<int> indices, IEnumerable<double> truth)
        {
            var x = new DenseVector(source.ToArray());
            var t = new DenseVector(truth.ToArray());
            var slice = Helpers.Slice(x, indices);
            Assert.AreEqual(t, slice);
        }

        [Test]
        public void Test_Matrix_Slicing_With_Indices()
        {
            var x = new DenseMatrix(new[,]{
                { 0d, 10 },
                { 1d, 20 },
                { 2d, 30 },
                { 3d, 40 },
                { 4d, 50 },
                { 5d, 60 },
                { 6d, 70 },
            });

            var indices = new[] { 6, 4, 2, 4, 1 };

            var truth = new DenseMatrix(new[,]{
                { 1d, 20 },
                { 2d, 30 },
                { 4d, 50 },
                { 6d, 70 },
            });

            var actual = Helpers.Slice(x, indices);

            Assert.AreEqual(truth, actual);
        }


        [Test]
        public void Test_Enumerable_Segmentation_1()
        {
            var x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.Throws(typeof(InvalidOperationException),
                () => Helpers.Segment(x, 1)
            );
        }

        [Test]
        public void Test_Enumerable_Segmentation_2()
        {
            var x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = Helpers.Segment(x, 2);
            var truth = new Range[] 
            {
                Range.Make(1, 5),
                Range.Make(5, 9.01),
            };

            Assert.AreEqual(truth.Length, ranges.Length);
            for (int i = 0; i < truth.Length; i++)
            {
                Assert.AreEqual(truth[i].Min, ranges[i].Min);
                Assert.AreEqual(truth[i].Max, ranges[i].Max);
            }
        }

        [Test]
        public void Test_Enumerable_Segmentation_3()
        {
            var x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = Helpers.Segment(x, 3);
            var segmentsize = 8d / 3d;
            var truth = new Range[] 
            {
                Range.Make(1, 1 + segmentsize),
                Range.Make(1 + segmentsize, 1 + 2 * segmentsize),
                Range.Make(1 + 2 * segmentsize, 9.01),
            };

            Assert.AreEqual(truth.Length, ranges.Length);
            for (int i = 0; i < truth.Length; i++)
            {
                Assert.AreEqual(truth[i].Min, ranges[i].Min);
                Assert.AreEqual(truth[i].Max, ranges[i].Max);
            }
        }

        [Test]
        public void Test_Enumerable_Segmentation_4()
        {
            var x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = Helpers.Segment(x, 4);
            var segmentsize = 8d / 4d;
            var truth = new Range[] 
            {
                Range.Make(1, 1 + segmentsize),
                Range.Make(1 + segmentsize, 1 + 2 * segmentsize),
                Range.Make(1 + 2 * segmentsize, 1 + 3 * segmentsize),
                Range.Make( 1 + 3 * segmentsize, 9.01)
            };

            Assert.AreEqual(truth.Length, ranges.Length);
            for (int i = 0; i < truth.Length; i++)
            {
                Assert.AreEqual(truth[i].Min, ranges[i].Min);
                Assert.AreEqual(truth[i].Max, ranges[i].Max);
            }
        }
    }
}
