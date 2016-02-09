using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace numl.Serialization
{
    public class MatrixSerializer : ISerializer
    {
        public bool CanConvert(Type t)
        {
            return typeof(Matrix).IsAssignableFrom(t);
        }

        public object Read(TextReader reader)
        {
            if (reader.IsNull())
                return null;
            else
            {
                var objects = reader.ReadArray(new VectorSerializer());
                return Matrix.Stack((from v in objects select (Vector)v).ToArray());
            }
        }

        public void Write(TextWriter writer, object o)
        {
            if (o is Matrix)
                writer.WriteArray((o as Matrix).GetRows(), new VectorSerializer());
        }
    }
}
