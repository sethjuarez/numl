// file:	Supervised\Model.cs
//
// summary:	Implements the model class
using numl.Math;
using numl.Math.LinearAlgebra;
using numl.Math.Normalization;
using numl.Model;
using numl.Serialization;
using numl.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace numl.Supervised
{
    /// <summary>A model.</summary>
    public abstract class Model : IModel, IModelBase
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }

        /// <summary>
        /// Gets or Sets whether to perform feature normalization using the specified Feature Normalizer.
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

        /// <summary>
        /// Preprocessed the input vector.
        /// </summary>
        /// <param name="x">Input vector.</param>
        /// <returns>Vector.</returns>
        protected void Preprocess(Vector x)
        {
            if (this.NormalizeFeatures)
            {
                Vector xp = this.FeatureNormalizer.Normalize(x, this.FeatureProperties);

                for (int i = 0; i < x.Length; i++)
                    x[i] = xp[i];
            }
        }

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
        /// Predicts the raw label value
        /// </summary>
        /// <param name="o">Object to predict</param>
        /// <returns>Predicted value</returns>
        public object PredictValue(object o)
        {
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Empty label precludes prediction!");

            var y = Descriptor.Convert(o, false).ToVector();
            var val = Predict(y);
            var result = Descriptor.Label.Convert(val);
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
        /// <summary>Model persistence.</summary>
        /// <param name="file">The file to load.</param>
        public virtual void Save(string file)
        {
            if (File.Exists(file)) File.Delete(file);
            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(this);
        }

        /// <summary>Converts this object to json.</summary>
        /// <returns>This object as a string.</returns>
        public virtual string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                new JsonWriter(sw).Write(this);
            return sb.ToString();
        }
    }
}
