using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using Xunit;
using numl.Math.Linkers;
using numl.Math.Metrics;
using numl.Unsupervised;
using System.Collections.Generic;

namespace numl.Tests.UnsupervisedTests
{
    [Trait("Category", "Unsupervised")]
    public class HierarchicalClusteringTests
    {
        [Fact]
        public void Cluster_Student()
        {
            Student[] students = Student.GetData().Take(20).ToArray();
            HClusterModel cluster = new HClusterModel();
            Descriptor descriptor = Descriptor.Create<Student>();
            CentroidLinker linker = new CentroidLinker(new EuclidianDistance());
             Cluster root = cluster.Generate(descriptor, students, linker);
        }
    }
}
