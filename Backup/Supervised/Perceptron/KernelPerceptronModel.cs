// file:	Supervised\Perceptron\KernelPerceptronModel.cs
//
// summary:	Implements the kernel perceptron model class
using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.Perceptron
{
    /// <summary>A data Model for the kernel perceptron.</summary>
    [Serializable]
    public class KernelPerceptronModel : Model
    {
        /// <summary>Gets or sets the kernel.</summary>
        /// <value>The kernel.</value>
        public IKernel Kernel { get; set; }
        /// <summary>Gets or sets the y coordinate.</summary>
        /// <value>The y coordinate.</value>
        public Vector Y { get; set; }
        /// <summary>Gets or sets a.</summary>
        /// <value>a.</value>
        public Vector A { get; set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        public Matrix X { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            var K = Kernel.Project(X, y);
            double v = 0;
            for (int i = 0; i < A.Length; i++)
                v += A[i] * Y[i] * K[i];

            return v;
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Kernel", Kernel.GetType().Name);
            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Vector>(writer, Y);
            Xml.Write<Vector>(writer, A);
            Xml.Write<Matrix>(writer, X);
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            var type = Ject.FindType(reader.GetAttribute("Kernel"));
            Kernel = (IKernel)Activator.CreateInstance(type);
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            Y = Xml.Read<Vector>(reader);
            A = Xml.Read<Vector>(reader);
            X = Xml.Read<Matrix>(reader);
        }
    }
}
