using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using numl.Tests.Data;
using numl.Unsupervised;
using numl.Math.Metrics;
using numl.Math.Probability;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;


namespace numl.Tests.UnsupervisedTests
{
    public class AB
    {
        [Feature]
        public double A { get; set; }
        [Feature]
        public double B { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", A, B);
        }
    }

    [TestFixture, Category("Unsupervised")]
    public class KMeansTests
    {
        private static Matrix GenerateData(int size)
        {
            // this creates [size] points in graph quadrant 1
            var A = Matrix.Create(size, 2, () => Sampling.GetNormal());
            A[0, VectorType.Col] -= 20;
            A[1, VectorType.Col] -= 20;

            // this creates [size] points in graph quadrant 3
            var B = Matrix.Create(size, 2, () => Sampling.GetNormal());
            B[0, VectorType.Col] += 20;
            B[1, VectorType.Col] += 20;

            // stack them
            var X = A.Stack(B);
            return X;
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Test_Numerical_KMeans(int size)
        {
            Matrix X = GenerateData(size);

            KMeans model = new KMeans();
            var assignment = model.Generate(X, 2, new EuclidianDistance());
            Assert.AreEqual(size * 2, assignment.Length);
            var a1 = assignment.First();
            var a2 = assignment.Last();
            for (int i = 0; i < size * 2; i++)
            {
                if (i < size)
                    Assert.AreEqual(a1, assignment[i]);
                else
                    Assert.AreEqual(a2, assignment[i]);
            }
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void Test_Object_KMeans(int size)
        {
            Matrix X = GenerateData(size);
            var objects = X.GetRows()
                           .Select(v => new AB { A = v[0], B = v[1] })
                           .ToArray();

            var descriptor = Descriptor.Create<AB>();

            KMeans model = new KMeans();
            var clusters = model.Generate(descriptor, objects, 2, new EuclidianDistance());
            Assert.AreEqual(2, clusters.Children.Length);
            Assert.AreEqual(size, clusters[0].Members.Length);
            Assert.AreEqual(size, clusters[1].Members.Length);
        }

        [Test]
        public void Test_Feed_KMeans()
        {
            int groups = 4;
            Feed[] feeds = Feed.GetData();
            Descriptor descriptor = Descriptor.Create<Feed>();
            KMeans kmeans = new KMeans();
            kmeans.Descriptor = descriptor;

            int[] grouping = kmeans.Generate(feeds, groups, new CosineDistance());

            for (int i = 0; i < grouping.Length; i++)
                feeds[i].Cluster = grouping[i];

        }
    }
}
