// file:	Supervised\Perceptron\PerceptronModel.cs
//
// summary:	Implements the perceptron model class
using System;
using System.Xml;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.Perceptron
{
    /// <summary>A data Model for the perceptron.</summary>
    public class PerceptronModel : Model
    {
        /// <summary>Gets or sets the w.</summary>
        /// <value>The w.</value>
        public Vector W { get; set; }
        /// <summary>Gets or sets the b.</summary>
        /// <value>The b.</value>
        public double B { get; set; }
        /// <summary>Gets or sets a value indicating whether the normalized.</summary>
        /// <value>true if normalized, false if not.</value>
        public bool Normalized { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            if (Normalized)
                y = y / y.Norm();

            return W.Dot(y) + B;
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("B", B.ToString("r"));
            writer.WriteAttributeString("Normalized", Normalized.ToString());

            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Vector>(writer, W);
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            B = double.Parse(reader.GetAttribute("B"));
            Normalized = bool.Parse(reader.GetAttribute("Normalized"));
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            W = Xml.Read<Vector>(reader);
        }
    }
}
