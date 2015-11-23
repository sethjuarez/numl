using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;

using numl.Math.Linkers;
using numl.Math.Metrics;
using numl.Unsupervised;
using System.Collections.Generic;

namespace numl.Tests.UnsupervisedTests
{
    [TestClass, TestCategory("Unsupervised")]
    public class HierarchicalClusteringTests
    {
        [TestMethod]
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
