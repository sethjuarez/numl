using System.Linq;
using numl.Data;
using Xunit;

namespace numl.Tests.DataTests
{
    public class Vertex : IVertex
    {
        static int _id = 0;
        public static void Reset() => _id = 0;
        public Vertex() { Id = ++_id; }
        public int Id { get; set; }

        public string Label { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Vertex)
                return ((Vertex)obj).Id == Id && ((Vertex)obj).Label == Label;
            else
                return false;
        }
    }

    public class Edge : IEdge
    {
        public int ChildId { get; set; }

        public int ParentId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Edge)
                return ((Edge)obj).ChildId == ChildId && ((Edge)obj).ParentId == ParentId;
            else
                return false;
        }
    }


    [Trait("Category", "Data")]
    public class GraphTests
    {
        [Fact]
        public void OutEdgesVertexTest()
        {
            Vertex.Reset();
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
            Assert.Equal(2, edges.Length);

            Assert.True(edges.Select(e => e.ParentId).Distinct().Count() == 1);
            Assert.Equal(1, edges.Select(e => e.ParentId).Distinct().First());
            Assert.True(edges.Select(e => e.ChildId).Contains(5));
            Assert.True(edges.Select(e => e.ChildId).Contains(2));

        }

        [Fact]
        public void ChildVertexTest()
        {
            Vertex.Reset();
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
            Assert.Equal(2, children.Length);

            Assert.True(children.Select(e => e.Id).ToArray().Contains(5));
            Assert.True(children.Select(e => e.Id).ToArray().Contains(2));

        }

        [Fact]
        public void InEdgesVertexTest()
        {
            Vertex.Reset();
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
            Assert.Equal(2, edges.Length);

            Assert.True(edges.Select(e => e.ChildId).Distinct().Count() == 1);
            Assert.Equal(1, edges.Select(e => e.ChildId).Distinct().First());
            Assert.True(edges.Select(e => e.ParentId).ToArray().Contains(5));
            Assert.True(edges.Select(e => e.ParentId).ToArray().Contains(2));

        }

        [Fact]
        public void ParentVertexTest()
        {
            Vertex.Reset();
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
            Assert.Equal(2, parents.Length);

            Assert.True(parents.Select(e => e.Id).ToArray().Contains(5));
            Assert.True(parents.Select(e => e.Id).ToArray().Contains(2));
        }

        [Fact]
        public void RemoveVertexTest()
        {
            Vertex.Reset();
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
            g.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            g.RemoveVertex(vertex1);

            Assert.Equal(0, g.GetVertices().Where(v => v.Id == 1).Count());
            Assert.Equal(0, g.GetEdges().Where(e => e.ParentId == 1 || e.ChildId == 1).Count());
        }

        [Fact]
        public void RemoveEdgeTest()
        {
            Vertex.Reset();
            Graph g = new Graph();
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

            Assert.Equal(0, g.GetEdges().Where(e => e.ParentId == 1 && e.ChildId == 5).Count());

        }

        [Fact]
        public void EqualsTest()
        {
            Vertex.Reset();
            Graph g1 = new Graph();
            g1.AddVertex(new Vertex()); // 1
            g1.AddVertex(new Vertex()); // 2
            g1.AddVertex(new Vertex()); // 3
            g1.AddVertex(new Vertex()); // 4
            g1.AddVertex(new Vertex()); // 5

            g1.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g1.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            Vertex.Reset();
            Graph g2 = new Graph();
            g2.AddVertex(new Vertex()); // 1
            g2.AddVertex(new Vertex()); // 2
            g2.AddVertex(new Vertex()); // 3
            g2.AddVertex(new Vertex()); // 4
            g2.AddVertex(new Vertex()); // 5

            g2.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g2.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g2.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            Assert.True(g1.Equals(g2));
        }

        [Fact]
        public void NotEqualsTest()
        {
            Vertex.Reset();
            Graph g1 = new Graph();
            g1.AddVertex(new Vertex()); // 1
            g1.AddVertex(new Vertex()); // 2
            g1.AddVertex(new Vertex()); // 3
            g1.AddVertex(new Vertex()); // 4
            g1.AddVertex(new Vertex()); // 5

            g1.AddEdge(new Edge { ParentId = 4, ChildId = 5 });
            g1.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            Vertex.Reset();
            Graph g2 = new Graph();
            g2.AddVertex(new Vertex()); // 1
            g2.AddVertex(new Vertex()); // 2
            g2.AddVertex(new Vertex()); // 3
            g2.AddVertex(new Vertex()); // 4
            g2.AddVertex(new Vertex()); // 5

            g2.AddEdge(new Edge { ParentId = 1, ChildId = 5 });
            g2.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 3 });
            g2.AddEdge(new Edge { ParentId = 3, ChildId = 1 });

            Assert.False(g1.Equals(g2));
        }

        [Fact]
        public void TreeEqualTest()
        {
            Vertex.Reset();
            Tree g1 = new Tree();
            var r1 = new Vertex();
            g1.Root = r1;
            g1.AddVertex(r1); // 1
            g1.AddVertex(new Vertex()); // 2
            g1.AddVertex(new Vertex()); // 3
            g1.AddVertex(new Vertex()); // 4
            g1.AddVertex(new Vertex()); // 5
            g1.AddVertex(new Vertex()); // 6
            g1.AddVertex(new Vertex()); // 7

            g1.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g1.AddEdge(new Edge { ParentId = 1, ChildId = 3 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 4 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 5 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 6 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 7 });

            Vertex.Reset();
            Tree g2 = new Tree();
            var r2 = new Vertex();
            g2.Root = r2;
            g2.AddVertex(r2); // 1
            g2.AddVertex(new Vertex()); // 2
            g2.AddVertex(new Vertex()); // 3
            g2.AddVertex(new Vertex()); // 4
            g2.AddVertex(new Vertex()); // 5
            g2.AddVertex(new Vertex()); // 6
            g2.AddVertex(new Vertex()); // 7

            g2.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g2.AddEdge(new Edge { ParentId = 1, ChildId = 3 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 4 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 5 });
            g2.AddEdge(new Edge { ParentId = 3, ChildId = 6 });
            g2.AddEdge(new Edge { ParentId = 3, ChildId = 7 });

            Assert.True(g1.Equals(g2));
        }

        [Fact]
        public void TreeNotEqualTest()
        {
            Vertex.Reset();
            Tree g1 = new Tree();
            var r1 = new Vertex();
            g1.Root = r1;
            g1.AddVertex(r1); // 1
            g1.AddVertex(new Vertex()); // 2
            g1.AddVertex(new Vertex()); // 3
            g1.AddVertex(new Vertex()); // 4
            g1.AddVertex(new Vertex()); // 5
            g1.AddVertex(new Vertex()); // 6
            g1.AddVertex(new Vertex()); // 7

            g1.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g1.AddEdge(new Edge { ParentId = 1, ChildId = 3 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 4 });
            g1.AddEdge(new Edge { ParentId = 2, ChildId = 5 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 6 });
            g1.AddEdge(new Edge { ParentId = 3, ChildId = 7 });

            Vertex.Reset();
            Tree g2 = new Tree();
            var r2 = new Vertex();
            g2.Root = r2;
            g2.AddVertex(r2); // 1
            g2.AddVertex(new Vertex()); // 2
            g2.AddVertex(new Vertex()); // 3
            g2.AddVertex(new Vertex()); // 4
            g2.AddVertex(new Vertex()); // 5
            g2.AddVertex(new Vertex()); // 6
            g2.AddVertex(new Vertex()); // 7

            g2.AddEdge(new Edge { ParentId = 1, ChildId = 2 });
            g2.AddEdge(new Edge { ParentId = 1, ChildId = 3 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 4 });
            g2.AddEdge(new Edge { ParentId = 2, ChildId = 5 });
            g2.AddEdge(new Edge { ParentId = 1, ChildId = 6 });
            g2.AddEdge(new Edge { ParentId = 3, ChildId = 7 });

            Assert.False(g1.Equals(g2));
        }


    }
}
