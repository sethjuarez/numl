using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;

namespace numl.Supervised
{
    public interface IGenerator
    {
        Descriptor Descriptor { get; set; }
        IModel Generate(Descriptor descriptor, IEnumerable<object> examples);
        IModel Generate<T>(IEnumerable<T> examples) where T : class;
        IModel Generate(Matrix x, Vector y);
    }
}
