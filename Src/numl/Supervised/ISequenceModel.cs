using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Supervised
{
    /// <summary>
    /// Implements a Sequence model.
    /// </summary>
    public interface ISequenceModel : IModel
    {
        /// <summary>
        /// Predicts the given example.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        Vector PredictSequence(Vector x);
    }
}
