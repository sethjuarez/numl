using System;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    /// <summary>
    /// Graph class.
    /// </summary>
    public class Graph
    {
        private readonly Dictionary<int, IVertex> _vertices;
        private readonly Dictionary<int, Dictionary<int, IEdge>> _edges;

        /// <summary>
        /// Initializes a new Graph.
        /// </summary>
        public Graph()
        {
            _vertices = new Dictionary<int, IVertex>();
            _edges = new Dictionary<int, Dictionary<int, IEdge>>();
        }

        /// <summary>
        /// Adds the specified IVertex to the current Graph.
        /// </summary>
        /// <param name="v">IVertex object to add.</param>
        public void AddVertex(IVertex v)
        {
            _vertices[v.Id] = v;
        }

        /// <summary>
        /// Gets the IVertex associated with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the IVertex to return.</param>
        /// <returns>IVertex</returns>
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

        /// <summary>
        /// Removes the specified Vertex and its associated edges from the Graph.
        /// </summary>
        /// <param name="v">IVertex to remove.</param>
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

        /// <summary>
        /// Inserts the Edge object to the Graph.
        /// <para>Connecting IVertex objects should already be present in the graph before attempting to add a connection.</para>
        /// </summary>
        /// <param name="edge">IEdge object to add.</param>
        public void AddEdge(IEdge edge)
        {
            if (_vertices.ContainsKey(edge.ParentId) && _vertices.ContainsKey(edge.ChildId))
                _edges.AddOrUpdate(edge.ParentId, edge.ChildId, edge);
            else
                throw new InvalidOperationException("Invalid vertex index specified in edge");
        }
        
        /// <summary>
        /// Removes the Edge object from the graph.
        /// </summary>
        /// <param name="edge">IEdge object to remove.</param>
        public void RemoveEdge(IEdge edge)
        {
            _edges[edge.ParentId].Remove(edge.ChildId);
        }

        /// <summary>
        /// Gets the efferent or outbound connections from the specified IVertex object. 
        /// </summary>
        /// <param name="v">IVertex object to return edges for.</param>
        /// <returns>IEnumerable&lt;IEdge&gt;</returns>
        public IEnumerable<IEdge> GetOutEdges(IVertex v)
        {
            foreach (var edges in _edges[v.Id])
                yield return edges.Value;
        }

        /// <summary>
        /// Gets the afferent or inbound connections from the specified IVertex object. 
        /// </summary>
        /// <param name="v">IVertex object to return edges for.</param>
        /// <returns>IEnumerable&lt;IEdge&gt;</returns>
        public IEnumerable<IEdge> GetInEdges(IVertex v)
        {
            foreach (var edges in _edges)
                foreach (var e in edges.Value)
                    if (e.Value.ChildId == v.Id)
                        yield return e.Value;
        }

        /// <summary>
        /// Gets the efferent or child vertices from the specified IVertex object. 
        /// </summary>
        /// <param name="v">IVertex object to return child vertices for.</param>
        /// <returns>IEnumerable&lt;IVertex&gt;</returns>
        public IEnumerable<IVertex> GetChildren(IVertex v)
        {
            foreach (var edges in GetOutEdges(v))
                yield return _vertices[edges.ChildId];
        }

        /// <summary>
        /// Gets the afferent or parent vertices from the specified IVertex object. 
        /// </summary>
        /// <param name="v">IVertex object to return parent vertices for.</param>
        /// <returns>IEnumerable&lt;IVertex&gt;</returns>
        public IEnumerable<IVertex> GetParents(IVertex v)
        {
            foreach (var edges in GetInEdges(v))
                yield return _vertices[edges.ParentId];
        }

        /// <summary>
        /// Returns all IVertex objects in the current graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IVertex> GetVertices()
        {
            foreach (var vertices in _vertices)
                yield return vertices.Value;
        }

        /// <summary>
        /// Returns all IEdge objects in the current graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEdge> GetEdges()
        {
            foreach (var edges in _edges)
                foreach (var e in edges.Value)
                    yield return e.Value;
        }

        /// <summary>
        /// Returns the hash code for this Graph object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current Graph is equal to the specified Graph object.
        /// </summary>
        /// <param name="obj">Graph object.</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is Graph)
            {
                var g = obj as Graph;
                foreach (int id in _vertices.Keys)
                {
                    if (!g._vertices.ContainsKey(id))
                        return false;
                    if (!g._vertices[id].Equals(_vertices[id]))
                        return false;
                }

                foreach(int from in _edges.Keys)
                {
                    if (!g._edges.ContainsKey(from))
                        return false;

                    foreach (var to in _edges[from].Keys)
                    {
                        if (!g._edges[from].ContainsKey(to))
                            return false;
                        if (!g._edges[from][to].Equals(_edges[from][to]))
                            return false;
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}
