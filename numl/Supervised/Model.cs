// file:	Supervised\Model.cs
//
// summary:	Implements the model class
using System;
using System.IO;
using numl.Utils;
using numl.Model;
using System.Linq;

using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised
{
    /// <summary>A model.</summary>
    public abstract class Model : IModel
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }

        /// <summary>
        /// Gets or Sets whether to perform feature normalisation using the specified Feature Normalizer.
        /// </summary>
        public bool NormalizeFeatures { get; set; }

        /// <summary>
        /// Feature normalizer to use over each item.
        /// </summary>
        public numl.Math.Normalization.INormalizer FeatureNormalizer { get; set; }

        /// <summary>
        /// Feature properties from the original item set.
        /// </summary>
        public numl.Math.Summary FeatureProperties { get; set; }

        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public abstract double Predict(Vector y);

        /// <summary>
        /// Predicts the given examples.
        /// </summary>
        /// <param name="x">Matrix of examples to predict.</param>
        /// <returns>Vector of predictions.</returns>
        public virtual Vector Predict(Matrix x)
        {
            Vector v = Vector.Zeros(x.Rows);

            for (int row = 0; row < x.Rows; row++)
                v[row] = this.Predict(x[row, VectorType.Row]);

            return v;
        }

        /// <summary>Predicts the given o.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="o">The object to process.</param>
        /// <returns>An object.</returns>
        public object Predict(object o)
        {
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Empty label precludes prediction!");

            var y = Descriptor.Convert(o, false).ToVector();
            var val = Predict(y);
            var result = Descriptor.Label.Convert(val);
            Ject.Set(o, Descriptor.Label.Name, result);
            return o;
        }

        /// <summary>
        /// Predicts all the given objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="objects">The objects to process.</param>
        /// <returns>Array of object predictions.</returns>
        public object[] Predict(object[] objects)
        {
            object[] result = new object[objects.Count()];
            for (int x = 0; x < result.Length; x++)
            {
                result[x] = this.Predict(objects[x]);
            }
            return result;
        }

        /// <summary>Predicts the given o.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="o">The object to process.</param>
        /// <returns>A T.</returns>
        public T Predict<T>(T o)
        {
            return (T)Predict((object)o);
        }

        // ----- saving stuff
        /// <summary>Model persistance.</summary>
        /// <param name="file">The file to load.</param>
        public virtual void Save(string file)
        {
            throw new NotImplementedException();
        }
        /// <summary>Saves the given stream.</summary>
        /// <param name="stream">The stream to load.</param>
        public virtual void Save(Stream stream)
        {
            throw new NotImplementedException();
        }
        /// <summary>Converts this object to an XML.</summary>
        /// <returns>This object as a string.</returns>
        public virtual string ToXml()
        {
            throw new NotImplementedException();
        }
        /// <summary>Loads the given stream.</summary>
        /// <param name="file">The file to load.</param>
        /// <returns>An IModel.</returns>
        public virtual IModel Load(string file)
        {
            throw new NotImplementedException();
        }
        /// <summary>Loads the given stream.</summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>An IModel.</returns>
        public virtual IModel Load(Stream stream)
        {
            throw new NotImplementedException();
        }
        /// <summary>Loads an XML.</summary>
        /// <param name="xml">The XML.</param>
        /// <returns>The XML.</returns>
        public virtual IModel LoadXml(string xml)
        {
            throw new NotImplementedException();
        }

    }
}
