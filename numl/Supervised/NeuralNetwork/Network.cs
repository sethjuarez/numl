// file:	Supervised\NeuralNetwork\Network.cs
//
// summary:	Implements the network class
using System;
using numl.Model;
using System.Linq;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A network.</summary>
    [XmlRoot("Network")]
    public class Network : IXmlSerializable
    {
        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        public Node[] In { get; set; }
        /// <summary>Gets or sets the out.</summary>
        /// <value>The out.</value>
        public Node[] Out { get; set; }
        /// <summary>Defaults.</summary>
        /// <param name="d">The Descriptor to process.</param>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="activation">The activation.</param>
        /// <returns>A Network.</returns>
        public static Network Default(Descriptor d, Matrix x, Vector y, IFunction activation)
        {
            Network nn = new Network();
            // set output to number of choices of available
            // 1 if only two choices
            int distinct = y.Distinct().Count();
            int output = distinct > 2 ? distinct : 1;
            // identity funciton for bias nodes
            IFunction ident = new Ident();

            // set number of hidden units to (Input + Hidden) * 2/3 as basic best guess. 
            int hidden = (int)System.Math.Ceiling((decimal)(x.Cols + output) * 2m / 3m);

            // creating input nodes
            nn.In = new Node[x.Cols + 1];
            nn.In[0] = new Node { Label = "B0", Activation = ident };
            for (int i = 1; i < x.Cols + 1; i++)
                nn.In[i] = new Node { Label = d.ColumnAt(i - 1), Activation = ident };

            // creating hidden nodes
            Node[] h = new Node[hidden + 1];
            h[0] = new Node { Label = "B1", Activation = ident };
            for (int i = 1; i < hidden + 1; i++)
                h[i] = new Node { Label = String.Format("H{0}", i), Activation = activation };

            // creating output nodes
            nn.Out = new Node[output];
            for (int i = 0; i < output; i++)
                nn.Out[i] = new Node { Label = GetLabel(i, d), Activation = activation };

            // link input to hidden. Note: there are
            // no inputs to the hidden bias node
            for (int i = 1; i < h.Length; i++)
                for (int j = 0; j < nn.In.Length; j++)
                    Edge.Create(nn.In[j], h[i]);

            // link from hidden to output (full)
            for (int i = 0; i < nn.Out.Length; i++)
                for (int j = 0; j < h.Length; j++)
                    Edge.Create(h[j], nn.Out[i]);

            return nn;
        }
        /// <summary>Gets a label.</summary>
        /// <param name="n">The Node to process.</param>
        /// <param name="d">The Descriptor to process.</param>
        /// <returns>The label.</returns>
        private static string GetLabel(int n, Descriptor d)
        {
            if (d.Label.Type.IsEnum)
                return Enum.GetName(d.Label.Type, n);
            else if (d.Label is StringProperty && ((StringProperty)d.Label).AsEnum)
                return ((StringProperty)d.Label).Dictionary[n];
            else return d.Label.Name;
        }
        /// <summary>Forwards the given x coordinate.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Vector to process.</param>
        public void Forward(Vector x)
        {
            if (In.Length != x.Length + 1)
                throw new InvalidOperationException("Input nodes not aligned to input vector");

            // set input
            for (int i = 0; i < In.Length; i++)
                In[i].Input = In[i].Output = i == 0 ? 1 : x[i - 1];
            // evaluate
            for (int i = 0; i < Out.Length; i++)
                Out[i].Evaluate();
        }
        /// <summary>Backs.</summary>
        /// <param name="t">The double to process.</param>
        /// <param name="learningRate">The learning rate.</param>
        public void Back(double t, double learningRate)
        {
            // propagate error gradients
            for (int i = 0; i < In.Length; i++)
                In[i].Error(t);

            // reset weights
            for (int i = 0; i < Out.Length; i++)
                Out[i].Update(learningRate);
        }
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable
        /// interface, you should return null (Nothing in Visual Basic) from this method, and instead, if
        /// specifying a custom schema is required, apply the
        /// <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the
        /// object that is produced by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" />
        /// method and consumed by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" />
        /// method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            XmlSerializer nSerializer = new XmlSerializer(typeof(Node));
            XmlSerializer eSerializer = new XmlSerializer(typeof(Edge));

            reader.MoveToContent();


            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            int length = 0;
            reader.ReadStartElement();
            length = int.Parse(reader.GetAttribute("Length"));
            reader.ReadStartElement("Nodes");
            for (int i = 0; i < length; i++)
            {
                var node = (Node)nSerializer.Deserialize(reader);
                nodes.Add(node.Id, node);
                reader.Read();
            }
            reader.ReadEndElement();

            length = int.Parse(reader.GetAttribute("Length"));
            reader.ReadStartElement("Edges");
            for (int i = 0; i < length; i++)
            {
                var edge = (Edge)eSerializer.Deserialize(reader);
                reader.Read();

                edge.Source = nodes[edge.SourceId];
                edge.Target = nodes[edge.TargetId];

                edge.Source.Out.Add(edge);
                edge.Target.In.Add(edge);
            }
            reader.ReadEndElement();

            length = int.Parse(reader.GetAttribute("Length"));
            reader.ReadStartElement("In");
            In = new Node[length];
            for (int i = 0; i < length; i++)
            {
                reader.MoveToContent();
                In[i] = nodes[reader.GetAttribute("Id")];
                reader.Read();

            }
            reader.ReadEndElement();

            length = int.Parse(reader.GetAttribute("Length"));
            reader.ReadStartElement("Out");
            Out = new Node[length];
            for (int i = 0; i < length; i++)
            {
                reader.MoveToContent();
                Out[i] = nodes[reader.GetAttribute("Id")];
                reader.Read();

            }
            reader.ReadEndElement();
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer nSerializer = new XmlSerializer(typeof(Node));
            XmlSerializer eSerializer = new XmlSerializer(typeof(Edge));

            var nodes = GetNodes().ToArray();
            writer.WriteStartElement("Nodes");
            writer.WriteAttributeString("Length", nodes.Length.ToString());
            foreach (var node in nodes)
                nSerializer.Serialize(writer, node);

            writer.WriteEndElement();

            var edges = GetEdges().ToArray();
            writer.WriteStartElement("Edges");
            writer.WriteAttributeString("Length", edges.Length.ToString());
            foreach (var edge in edges)
                eSerializer.Serialize(writer, edge);

            writer.WriteEndElement();

            writer.WriteStartElement("In");
            writer.WriteAttributeString("Length", In.Length.ToString());
            for (int i = 0; i < In.Length; i++)
            {
                writer.WriteStartElement("Node");
                writer.WriteAttributeString("Id", In[i].Id);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Out");
            writer.WriteAttributeString("Length", Out.Length.ToString());
            for (int i = 0; i < Out.Length; i++)
            {
                writer.WriteStartElement("Node");
                writer.WriteAttributeString("Id", Out[i].Id);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }


        /// <summary>The nodes.</summary>
        private HashSet<string> _nodes;
        /// <summary>Gets the nodes in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the nodes in this collection.
        /// </returns>
        public IEnumerable<Node> GetNodes()
        {
            if (_nodes == null) _nodes = new HashSet<string>();
            else _nodes.Clear();

            foreach (var node in Out)
            {
                _nodes.Add(node.Id);
                yield return node;
                foreach (var n in GetNodes(node))
                {
                    if (!_nodes.Contains(n.Id))
                    {
                        _nodes.Add(n.Id);
                        yield return n;
                    }
                }
            }
        }
        /// <summary>Gets the nodes in this collection.</summary>
        /// <param name="n">The Node to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the nodes in this collection.
        /// </returns>
        private IEnumerable<Node> GetNodes(Node n)
        {
            foreach (var edge in n.In)
            {
                yield return edge.Source;
                foreach (var node in GetNodes(edge.Source))
                    yield return node;
            }
        }


        /// <summary>The edges.</summary>
        private HashSet<Tuple<string, string>> _edges;
        /// <summary>Gets the edges in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the edges in this collection.
        /// </returns>
        public IEnumerable<Edge> GetEdges()
        {
            if (_edges == null) _edges = new HashSet<Tuple<string, string>>();
            else _edges.Clear();

            foreach (var node in Out)
            {
                foreach (var edge in GetEdges(node))
                {
                    var key = new Tuple<string, string>(edge.Source.Id, edge.Target.Id);
                    if (!_edges.Contains(key))
                    {
                        _edges.Add(key);
                        yield return edge;
                    }
                }
            }
        }
        /// <summary>Gets the edges in this collection.</summary>
        /// <param name="n">The Node to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the edges in this collection.
        /// </returns>
        private IEnumerable<Edge> GetEdges(Node n)
        {
            foreach (var edge in n.In)
            {
                yield return edge;
                foreach (var e in GetEdges(edge.Source))
                    yield return e;
            }
        }
    }
}
