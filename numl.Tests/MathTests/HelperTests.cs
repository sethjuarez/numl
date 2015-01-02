using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Math;
using numl.Math.Information;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;

namespace numl.Tests.MathTests
{
    [TestFixture, Category("Math")]
    public class HelperTests
    {
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 1.2d, 2.2, 2.1, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 1.2d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new double[] { })]
        public void Test_Enumerable_Slicing_With_Where_LessThanEqual(IEnumerable<double> source, double pivot, IEnumerable<double> truth)
        {
            var slice = new Vector(source).Slice(d => d <= pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 5.2d, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 2.2d, 2.1, 5.2, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        public void Test_Enumerable_Slicing_With_Where_GreaterThan(IEnumerable<double> source, double pivot, IEnumerable<double> truth)
        {
            var slice = new Vector(source).Slice(d => d > pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 0, 1, 2, 4 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 0, 4 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new int[] { })]
        public void Test_Enumerable_Indices_With_Where_LessThanEqual(IEnumerable<double> source, double pivot, IEnumerable<int> truth)
        {
            var slice = new Vector(source).Indices(d => d <= pivot);
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 5, new[] { 3, 5, 6 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 2.0, new[] { 1, 2, 3, 5, 6 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, 0.1, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        public void Test_Enumerable_Indices_With_Where_GreaterThan(IEnumerable<double> source, double pivot, IEnumerable<int> truth)
        {
            var slice = new Vector(source).Indices(d => d > pivot);
            Assert.AreEqual(truth, slice);
        }


        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 2, 3, 4 }, new[] { 2.1d, 5.2, 0.2, })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 4, 2, 4, 2 }, new[] { 2.1d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 0, 6, 4, 3, 2, 5, 1 }, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 43, 9, 12 }, new double[] { })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new int[] { }, new double[] { })]
        public void Test_Enumerable_Slicing_With_Indices(IEnumerable<double> source, IEnumerable<int> indices, IEnumerable<double> truth)
        {
            var slice = new Vector(source).Slice(indices).ToArray();
            Assert.AreEqual(truth, slice);
        }

        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 2, 3, 4 }, new[] { 2.1d, 5.2, 0.2, })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 4, 2, 4, 2 }, new[] { 2.1d, 0.2 })]
        [TestCase(new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 }, new[] { 0, 6, 4, 3, 2, 5, 1 }, new[] { 1.2d, 2.2, 2.1, 5.2, 0.2, 6.7, 8.8 })]
        public void Test_Vector_Slicing_With_Indices(IEnumerable<double> source, IEnumerable<int> indices, IEnumerable<double> truth)
        {
            var x = new Vector(source);
            var t = new Vector(truth);
            var slice = x.Slice(indices);
            Assert.AreEqual(t, slice);
        }

        [Test]
        public void Test_Matrix_Slicing_With_Indices()
        {
            Matrix x = new[,]{
                { 0d, 10 },
                { 1d, 20 },
                { 2d, 30 },
                { 3d, 40 },
                { 4d, 50 },
                { 5d, 60 },
                { 6d, 70 },
            };

            var indices = new[] { 6, 4, 2, 4, 1 };

            Matrix truth = new[,]{
                { 1d, 20 },
                { 2d, 30 },
                { 4d, 50 },
                { 6d, 70 },
            };

            var actual = x.Slice(indices);

            Assert.AreEqual(truth, actual);
        }


        [Test]
        public void Test_Enumerable_Segmentation_1()
        {
            Vector x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.Throws(typeof(InvalidOperationException),
                () => x.Segment(1)
            );
        }

        [Test]
        public void Test_Enumerable_Segmentation_2()
        {
            Vector x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = x.Segment(2);
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
            Vector x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = x.Segment(3);
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
            Vector x = new[] { 1d, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ranges = x.Segment(4);
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

        [Test]
        public void Test_Range_Single_Testing()
        {
            for (int i = 0; i < 10000; i++)
            {
                var t = Sampling.GetUniform();
                Assert.IsTrue(Range.Make(t).Test(t));
            }
        }

        [Test]
        public void Test_Range_Multi_Testing()
        {
            for (int i = 0; i < 10000; i++)
            {
                var a = Sampling.GetUniform();
                var b = a + Sampling.GetUniform();
                var test = Sampling.GetUniform();

                // scale test to allowable range
                var t = ((b - a) * test) + a;

                Assert.IsTrue(Range.Make(a, b).Test(t));
            }
        }
    }
}
