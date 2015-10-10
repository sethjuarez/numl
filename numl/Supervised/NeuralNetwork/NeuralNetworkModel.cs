// file:	Supervised\NeuralNetwork\NeuralNetworkModel.cs
//
// summary:	Implements the neural network model class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Utils;
using numl.Preprocessing;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A data Model for the neural network.</summary>
    public class NeuralNetworkModel : Model
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        public Network Network { get; set; }

        /// <summary>
        /// Gets or sets the output layer function (i.e. Softmax).
        /// </summary>
        public IFunction OutputFunction { get; set; }

        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            Network.Forward(y);

            Vector output = Network.Out.Select(n => n.Output).ToVector();

            return (this.OutputFunction != null ? this.OutputFunction.Compute(output).Max() : output.Max());
        }

        /// <summary>Converts an object into its XML representation.</summary>
        /// <exception cref="NotImplementedException">Thrown when the requested operation is unimplemented.</exception>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();

            //writer.WriteAttributeString(nameof(NormalizeFeatures), this.NormalizeFeatures.ToString());
            //writer.WriteAttributeString(nameof(FeatureNormalizer), FeatureNormalizer.GetType().Name);
        }

        /// <summary>Generates an object from its XML representation.</summary>
        /// <exception cref="NotImplementedException">Thrown when the requested operation is unimplemented.</exception>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();

            //reader.MoveToContent();

            //this.NormalizeFeatures = bool.Parse(reader.GetAttribute(nameof(NormalizeFeatures)));

            //var normalizer = Ject.FindType(reader.GetAttribute(nameof(FeatureNormalizer)));
            //base.FeatureNormalizer = (IFeatureNormalizer)Activator.CreateInstance(normalizer);

            //reader.ReadStartElement();
        }
    }
}
