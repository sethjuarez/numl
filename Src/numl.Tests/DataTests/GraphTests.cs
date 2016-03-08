using numl.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.DataTests
{
    public class Vertex : IVertex
    {
        static int _id = 0;
        public Vertex() { Id = ++_id; }
        public int Id { get; set; }

        public string Label { get; set; }
    }

    public class Edge : IEdge
    {
        public int ChildId { get; set; }

        public int ParentId { get; set; }
    }

    [TestFixture]
    public class GraphTests
    {
        [Test]
        public void OutEdgesVertexTest()
        {
            Graph g = new Graph();
            var vertex1 = new Vertex();
            g.AddVertex(vertex1); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            g.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });

            var edges = g.GetOutEdges(vertex1).ToArray();
            Assert.AreEqual(2, edges.Length);

            Assert.IsTrue(edges.Select(e => e.ParentId).Distinct().Count() == 1);
            Assert.AreEqual(1, edges.Select(e => e.ParentId).Distinct().First());
            Assert.IsTrue(edges.Select(e => e.ChildId).ToArray().Contains(5));
            Assert.IsTrue(edges.Select(e => e.ChildId).ToArray().Contains(2));

        }

        [Test]
        public void ChildVertexTest()
        {
            Graph g = new Graph();
            var vertex1 = new Vertex();
            g.AddVertex(vertex1); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            g.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });

            var children = g.GetChildren(vertex1).ToArray();
            Assert.AreEqual(2, children.Length);

            Assert.IsTrue(children.Select(e => e.Id).ToArray().Contains(5));
            Assert.IsTrue(children.Select(e => e.Id).ToArray().Contains(2));

        }

        [Test]
        public void InEdgesVertexTest()
        {
            Graph g = new Graph();
            var vertex1 = new Vertex();
            g.AddVertex(vertex1); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            g.AddEdge(new Edge { ParentId = 5, ChildId = 1 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 1 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });

            var edges = g.GetInEdges(vertex1).ToArray();
            Assert.AreEqual(2, edges.Length);

            Assert.IsTrue(edges.Select(e => e.ChildId).Distinct().Count() == 1);
            Assert.AreEqual(1, edges.Select(e => e.ChildId).Distinct().First());
            Assert.IsTrue(edges.Select(e => e.ParentId).ToArray().Contains(5));
            Assert.IsTrue(edges.Select(e => e.ParentId).ToArray().Contains(2));

        }

        [Test]
        public void ParentVertexTest()
        {
            Graph g = new Graph();
            var vertex1 = new Vertex();
            g.AddVertex(vertex1); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            g.AddEdge(new Edge { ParentId = 5, ChildId = 1 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 1 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });

            var parents = g.GetParents(vertex1).ToArray();
            Assert.AreEqual(2, parents.Length);

            Assert.IsTrue(parents.Select(e => e.Id).ToArray().Contains(5));
            Assert.IsTrue(parents.Select(e => e.Id).ToArray().Contains(2));
        }
    }
}
