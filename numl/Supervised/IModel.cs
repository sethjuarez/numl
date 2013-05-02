using System;
using System.IO;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised
{
    public interface IModel
    {
        double Predict(Vector y);
        T Predict<T>(T o);
        object Predict(object o);
        Descriptor Descriptor { get; set; }

        // Model persistance
        void Save(string file);
        void Save(Stream stream);
        IModel Load(string file);
        IModel Load(Stream stream);
    }
}