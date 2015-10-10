using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using numl.Utils;

namespace numl.Features
{
    /// <summary>
    /// FeatureProperties class.
    /// </summary>
    public class FeatureProperties : IXmlSerializable
    {
        /// <summary>
        /// Vector of all columns and the average values for each.
        /// </summary>
        public Vector Average { get; set; }

        /// <summary>
        /// Vector of all columns and the min values for each.
        /// </summary>
        public Vector Minimum { get; set; }

        /// <summary>
        /// Vector of all columns and the median values for each.
        /// </summary>
        public Vector Median { get; set; }

        /// <summary>
        /// Vector of all columns and the max values for each.
        /// </summary>
        public Vector Maximum { get; set; }

        /// <summary>
        /// Vector of all columns and the standard deviation for each.
        /// </summary>
        public Vector StandardDeviation { get; set; }

        /// <summary>
        /// Instantiate new FeatureProperties object.
        /// </summary>
        public FeatureProperties()
        {
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
        /// <exception cref="TypeLoadException">Thrown when a Type Load error condition occurs.</exception>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();

            Average = Xml.Read<Vector>(reader, nameof(Average), false);
            Maximum = Xml.Read<Vector>(reader, nameof(Maximum), false);
            Minimum = Xml.Read<Vector>(reader, nameof(Minimum), false);
            Median = Xml.Read<Vector>(reader, nameof(Median), false);
            StandardDeviation = Xml.Read<Vector>(reader, nameof(StandardDeviation), false);

            reader.ReadEndElement();
        }

        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            Xml.Write<Vector>(writer, Average, nameof(Average));
            Xml.Write<Vector>(writer, Maximum, nameof(Maximum));
            Xml.Write<Vector>(writer, Minimum, nameof(Minimum));
            Xml.Write<Vector>(writer, Median, nameof(Median));
            Xml.Write<Vector>(writer, StandardDeviation, nameof(StandardDeviation));
        }
    }
}
