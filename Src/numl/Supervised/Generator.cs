// file:	Supervised\Generator.cs
//
// summary:	Implements the generator class
using System;
using numl.Model;
using System.Linq;

using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised
{
    /// <summary>A generator.</summary>
    public abstract class Generator : IGenerator
    {
        /// <summary>Event queue for all listeners interested in ModelChanged events.</summary>
        public event EventHandler<ModelEventArgs> ModelChanged;
        /// <summary>Raises the model event.</summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information to send to registered event handlers.</param>
        protected virtual void OnModelChanged(object sender, ModelEventArgs e)
        {
            EventHandler<ModelEventArgs> handler = ModelChanged;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }
        
        /// <summary>
        /// Gets or sets whether to perform feature normalisation using the specified Feature Normalizer.
        /// </summary>
        public bool NormalizeFeatures { get; set; }
        /// <summary>
        /// Gets or sets the feature normalizer to use for each item.
        /// </summary>
        public numl.Math.Normalization.INormalizer FeatureNormalizer { get; set; }

        /// <summary>
        /// Gets or sets the Feature properties from the original training set.
        /// </summary>
        public numl.Math.Summary FeatureProperties { get; set; }

        /// <summary>
        /// Initializes a new Generator instance.
        /// </summary>
        public Generator()
        {
            this.NormalizeFeatures = false;
            this.FeatureNormalizer = new numl.Math.Normalization.MinMaxNormalizer();
        }

        /// <summary>
        /// Override to perform custom pre-processing steps on the raw Matrix data.
        /// </summary>
        /// <param name="X">Matrix of examples.</param>
        /// <param name="y">Vector of values.</param>
        /// <returns></returns>
        public virtual void Preprocess(Matrix X, Vector y)
        {
            this.FeatureProperties = new numl.Math.Summary()
            {
                Average = X.Mean(VectorType.Row),
                StandardDeviation = X.StdDev(VectorType.Row),
                Minimum = X.Min(VectorType.Row),
                Maximum = X.Max(VectorType.Row),
                Median = X.Median(VectorType.Row)
            };

            if (this.NormalizeFeatures)
            {
                if (this.FeatureNormalizer != null)
                {
                    for (int i = 0; i < X.Rows; i++)
                    {
                        Vector vectors = this.FeatureNormalizer.Normalize(X[i, VectorType.Row], this.FeatureProperties);
                        for (int j = 0; j < X.Cols; j++)
                        {
                            X[i, j] = vectors[j];
                        }
                    }
                }
            }
        }

        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples">Example set.</param>
        /// <returns>Model.</returns>
        public IModel Generate(IEnumerable<object> examples)
        {
            if (examples.Count() == 0) throw new InvalidOperationException("Empty example set.");

            if (Descriptor == null)
                throw new InvalidOperationException("Descriptor is null");

            return Generate(Descriptor, examples);
        }

        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="description">The description.</param>
        /// <param name="examples">Example set.</param>
        /// <returns>Model.</returns>
        public IModel Generate(Descriptor description, IEnumerable<object> examples)
        {
            if (examples.Count() == 0) throw new InvalidOperationException("Empty example set.");

            Descriptor = description;
            if (Descriptor.Features == null || Descriptor.Features.Length == 0)
                throw new InvalidOperationException("Invalid descriptor: Empty feature set!");
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Invalid descriptor: Empty label!");

            var doubles = Descriptor.Convert(examples);
            var tuple = doubles.ToExamples();

            return Generate(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// Generate model from descriptor and examples
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="descriptor">Desriptor</param>
        /// <param name="examples">Examples</param>
        /// <returns>Model</returns>
        public IModel Generate<T>(Descriptor descriptor, IEnumerable<T> examples) where T : class
        {
            return Generate(descriptor, examples as IEnumerable<object>);
        }

        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public abstract IModel Generate(Matrix x, Vector y);

        
    }

    /// <summary>Additional information for model events.</summary>
    public class ModelEventArgs : EventArgs
    {
        /// <summary>Constructor.</summary>
        /// <param name="model">The model.</param>
        /// <param name="message">(Optional) the message.</param>
        public ModelEventArgs(IModel model, string message = "")
        {
            Message = message;
            Model = model;
        }
        /// <summary>Gets or sets the model.</summary>
        /// <value>The model.</value>
        public IModel Model { get; private set; }
        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; private set; }
        /// <summary>Makes.</summary>
        /// <param name="model">The model.</param>
        /// <param name="message">(Optional) the message.</param>
        /// <returns>The ModelEventArgs.</returns>
        internal static ModelEventArgs Make(IModel model, string message = "")
        {
            return new ModelEventArgs(model, message);
        }
    }
}
