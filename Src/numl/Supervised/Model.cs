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
using numl.Math.Normalization;
using numl.Math;
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
        public INormalizer FeatureNormalizer { get; set; }

        /// <summary>
        /// Feature properties from the original item set.
        /// </summary>
        public Summary FeatureProperties { get; set; }

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
            numl.Serialization.JsonHelpers.Save(file, this);
        }
        
        /// <summary>Converts this object to json.</summary>
        /// <returns>This object as a string.</returns>
        public virtual string ToJson()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Loads a json string.</summary>
        /// <param name="json">The json string.</param>
        /// <returns>The Model.</returns>
        public virtual IModel LoadJson(string json)
        {
            throw new NotImplementedException();
        }

    }
}
