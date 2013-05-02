using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;


namespace numl.Math.Linkers
{
    public interface ILinker
    {
        double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y);
    }
}
