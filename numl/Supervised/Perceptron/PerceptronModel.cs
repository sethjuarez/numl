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
using numl.Features;
using numl.Preprocessing;

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
            writer.WriteAttributeString(nameof(B), B.ToString("r"));
            writer.WriteAttributeString(nameof(Normalized), Normalized.ToString());
            writer.WriteAttributeString(nameof(NormalizeFeatures), this.NormalizeFeatures.ToString());
            writer.WriteAttributeString(nameof(FeatureNormalizer), FeatureNormalizer.GetType().Name);


            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<FeatureProperties>(writer, base.FeatureProperties);
            Xml.Write<Vector>(writer, W, nameof(W));
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            B = double.Parse(reader.GetAttribute(nameof(B)));
            Normalized = bool.Parse(reader.GetAttribute(nameof(Normalized)));

            this.NormalizeFeatures = bool.Parse(reader.GetAttribute(nameof(NormalizeFeatures)));

            var normalizer = Ject.FindType(reader.GetAttribute(nameof(FeatureNormalizer)));
            base.FeatureNormalizer = (IFeatureNormalizer)Activator.CreateInstance(normalizer);

            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            base.FeatureProperties = Xml.Read<FeatureProperties>(reader, null, false);
            W = Xml.Read<Vector>(reader, nameof(W));
        }
    }
}
