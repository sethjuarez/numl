// file:	Math\Linkers\ILinker.cs
//
// summary:	Declares the ILinker interface
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;


namespace numl.Math.Linkers
{
    /// <summary>Interface for linker.</summary>
    public interface ILinker
    {
        /// <summary>Distances.</summary>
        /// <param name="x">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <param name="y">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <returns>A double.</returns>
        double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y);
    }
}
