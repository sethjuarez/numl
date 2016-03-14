using System.Linq;
using numl.Data;
using NUnit.Framework;

namespace numl.Tests.DataTests
{
    public class Vertex : IVertex
    {
        static int _id = 0;
        public static void Reset() => _id = 0;
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
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
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
            Assert.IsTrue(edges.Select(e => e.ChildId).Contains(5));
            Assert.IsTrue(edges.Select(e => e.ChildId).Contains(2));

        }

        [Test]
        public void ChildVertexTest()
        {
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
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
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
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
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
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

        [Test]
        public void RemoveVertexTest()
        {
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
            var vertex1 = new Vertex();
            g.AddVertex(vertex1); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            g.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            g.RemoveVertex(vertex1);

            Assert.AreEqual(0, g.GetVertices().Where(v => v.Id == 1).Count());
            Assert.AreEqual(0, g.GetEdges().Where(e => e.ParentId == 1 || e.ChildId == 1).Count());
        }

        [Test]
        public void RemoveEdgeTest()
        {
            Vertex.Reset();
            Graph<Vertex> g = new Graph<Vertex>();
            g.AddVertex(new Vertex()); // 1
            g.AddVertex(new Vertex()); // 2
            g.AddVertex(new Vertex()); // 3
            g.AddVertex(new Vertex()); // 4
            g.AddVertex(new Vertex()); // 5

            var edge = new Edge { ParentId = 1, ChildId = 5 };
            g.AddEdge(edge);
            g.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            g.RemoveEdge(edge);

            Assert.AreEqual(0, g.GetEdges().Where(e => e.ParentId == 1 && e.ChildId == 5).Count());

        }
    }
}
