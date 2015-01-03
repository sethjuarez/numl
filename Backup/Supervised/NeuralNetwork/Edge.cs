// file:	Supervised\NeuralNetwork\Edge.cs
//
// summary:	Implements the edge class
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>An edge.</summary>
    [XmlRoot("Edge")]
    public class Edge : IXmlSerializable
    {
        /// <summary>Default constructor.</summary>
        public Edge()
        {
            // random initialization
            // R. D. Reed and R. J. Marks II, "Neural Smithing: 
            // Supervised Learning in Feedforward Artificial 
            // Neural Networks", Mit Press, 1999. pg 57
            // selecting values from range [-a,+a] where 0.1 < a < 2
            Weight = (double)Sampling.GetUniform(1, 20) / 10d;
            if (Sampling.GetUniform() < .5)
                Weight *= -1;
        }
        /// <summary>Gets or sets the source for the.</summary>
        /// <value>The source.</value>
        public Node Source { get; set; }
        /// <summary>Gets or sets the identifier of the source.</summary>
        /// <value>The identifier of the source.</value>
        internal string SourceId { get; set; }
        /// <summary>Gets or sets the Target for the.</summary>
        /// <value>The target.</value>
        public Node Target { get; set; }
        /// <summary>Gets or sets the identifier of the target.</summary>
        /// <value>The identifier of the target.</value>
        internal string TargetId { get; set; }
        /// <summary>Gets or sets the weight.</summary>
        /// <value>The weight.</value>
        public double Weight { get; set; }
        /// <summary>Creates a new Edge.</summary>
        /// <param name="source">Source for the.</param>
        /// <param name="target">Target for the.</param>
        /// <returns>An Edge.</returns>
        public static Edge Create(Node source, Node target)
        {
            Edge e = new Edge { Source = source, Target = target };
            source.Out.Add(e);
            target.In.Add(e);
            return e;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} ---- {1} ----> {2}", Source, Weight, Target);
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
            reader.MoveToContent();
            SourceId = reader.GetAttribute("Source");
            TargetId = reader.GetAttribute("Target");
            Weight = double.Parse(reader.GetAttribute("Weight"));

        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Source", Source.Id);
            writer.WriteAttributeString("Target", Target.Id);
            writer.WriteAttributeString("Weight", Weight.ToString("r"));
        }
    }
}
