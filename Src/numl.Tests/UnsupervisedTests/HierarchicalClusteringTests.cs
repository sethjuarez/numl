using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using numl.Math.Linkers;
using numl.Math.Metrics;
using numl.Unsupervised;
using System.Collections.Generic;

namespace numl.Tests.UnsupervisedTests
{
    [TestFixture, Category("Unsupervised")]
    public class HierarchicalClusteringTests
    {
        [Test]
        public void Cluster_Student()
        {
            Student[] students = Student.GetData();
            HClusterModel cluster = new HClusterModel();
            Descriptor descriptor = Descriptor.Create<Student>();
            CentroidLinker linker = new CentroidLinker(new EuclidianDistance());
            Cluster root = cluster.Generate(descriptor, students, linker);
        }
    }
}
