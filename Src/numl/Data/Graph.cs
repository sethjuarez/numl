using System;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    public class Graph
    {
        private readonly Dictionary<int, IVertex> _vertices;
        private readonly Dictionary<int, Dictionary<int, IEdge>> _edges;

        public Graph()
        {
            _vertices = new Dictionary<int, IVertex>();
            _edges = new Dictionary<int, Dictionary<int, IEdge>>();
        }

        public void AddVertex(IVertex v)
        {
            _vertices[v.Id] = v;
        }

        public IVertex GetVertex(int id)
        {
            return this[id];
        }

        /// <summary>
        /// Gets the IVertex by the specified Id.
        /// </summary>
        /// <param name="id">The key of the specified IVertex to return.</param>
        /// <returns>IVertex.</returns>
        public IVertex this[int id]
        {
            get
            {
                if (_vertices.ContainsKey(id))
                    return _vertices[id];
                else
                    throw new InvalidOperationException($"Vertex {id} does not exist!");
            }
        }

        public void RemoveVertex(IVertex v)
        {
            // remove vertex
            _vertices.Remove(v.Id);

            // remove associated edges
            if (_edges.ContainsKey(v.Id))
                _edges.Remove(v.Id);

            foreach (var key in _edges.Keys)
                if (_edges[key].ContainsKey(v.Id))
                    _edges[key].Remove(v.Id);
        }

        public void AddEdge(IEdge edge)
        {
            if (_vertices.ContainsKey(edge.ParentId) && _vertices.ContainsKey(edge.ChildId))
                _edges.AddOrUpdate(edge.ParentId, edge.ChildId, edge);
            else
                throw new InvalidOperationException("Invalid vertex index specified in edge");
        }
        
        public void RemoveEdge(IEdge edge)
        {
            _edges[edge.ParentId].Remove(edge.ChildId);
        }

        public IEnumerable<IEdge> GetOutEdges(IVertex v)
        {
            foreach (var edges in _edges[v.Id])
                yield return edges.Value;
        }

        public IEnumerable<IEdge> GetInEdges(IVertex v)
        {
            foreach (var edges in _edges)
                foreach (var e in edges.Value)
                    if (e.Value.ChildId == v.Id)
                        yield return e.Value;
        }

        public IEnumerable<IVertex> GetChildren(IVertex v)
        {
            foreach (var edges in GetOutEdges(v))
                yield return _vertices[edges.ChildId];
        }

        public IEnumerable<IVertex> GetParents(IVertex v)
        {
            foreach (var edges in GetInEdges(v))
                yield return _vertices[edges.ParentId];
        }

        public IEnumerable<IVertex> GetVertices()
        {
            foreach (var vertices in _vertices)
                yield return vertices.Value;
        }

        public IEnumerable<IEdge> GetEdges()
        {
            foreach (var edges in _edges)
                foreach (var e in edges.Value)
                    yield return e.Value;
        }
    }
}
