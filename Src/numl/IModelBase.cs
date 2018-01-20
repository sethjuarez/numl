using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoTensor;
using numl.Math.Normalization;
using numl.Math;

namespace numl
{
    /// <summary>
    /// IModelBase interface.
    /// </summary>
    public interface IModelBase
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        Descriptor Descriptor { get; set; }

        /// <summary>
        /// Gets or Sets whether to perform feature normalisation using the specified feature normalizer (see <see cref="INormalizer"/>).
        /// </summary>
        bool NormalizeFeatures { get; set; }

        /// <summary>
        /// Feature normalizer to apply to each item.
        /// </summary>
        INormalizer FeatureNormalizer { get; set; }

        /// <summary>
        /// Feature properties of the original training set.
        /// </summary>
        Summary FeatureProperties { get; set; }
    }
}
