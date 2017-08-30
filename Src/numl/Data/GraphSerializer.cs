using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Serialization;

namespace numl.Data
{
    public class GraphSerializer : JsonSerializer<Graph>
    {
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull())
                return null;
            else
            {
                var g = Create() as Graph;
                var vertices = reader.ReadArrayProperty()
                                     .Value
                                     .Select(o => (IVertex)o);
                foreach (var v in vertices)
                    g.AddVertex(v);

                var edges = reader.ReadArrayProperty()
                                  .Value
                                  .Select(o => (IEdge)o);
                foreach (var e in edges)
                    g.AddEdge(e);

                return g;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var g = (Graph)value;
                writer.WriteArrayProperty("Vertices", g.GetVertices());
                writer.WriteArrayProperty("Edges", g.GetEdges());
            }
        }
    }
}
