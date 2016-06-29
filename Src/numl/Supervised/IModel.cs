// file:	Supervised\IModel.cs
//
// summary:	Declares the IModel interface
using System;
using System.IO;
using numl.Model;
using System.Linq;

using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Math.Normalization;
using numl.Math;
namespace numl.Supervised
{
    /// <summary>Interface for model.</summary>
    public interface IModel
    {
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        double Predict(Vector y);
        
        /// <summary>Predicts the given o.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="o">The object to process.</param>
        /// <returns>A T.</returns>
        T Predict<T>(T o);
        
        /// <summary>Predicts the given o.</summary>
        /// <param name="o">The object to process.</param>
        /// <returns>An object.</returns>
        object Predict(object o);

        /// <summary>
        /// Predicts the raw label value
        /// </summary>
        /// <param name="o">Object to predict</param>
        /// <returns>Predicted value</returns>
        object PredictValue(object o);

        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        Descriptor Descriptor { get; set; }

        /// <summary>
        /// Feature normalizer to apply
        /// </summary>
        INormalizer Normalizer { get; set; }

        /// <summary>
        /// Feature properties of the original item set.
        /// </summary>
        Summary Summary { get; set; }

        /// <summary>Saves the given IModel to a file.</summary>
        /// <param name="file">The file to load.</param>
        void Save(string file);
    }
}