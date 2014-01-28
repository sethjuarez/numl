using numl.Data;
using numl.Math.Linkers;
using numl.Math.Metrics;
using numl.Model;
using numl.Unsupervised;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.UnsupervisedTests
{
    [TestFixture]
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
