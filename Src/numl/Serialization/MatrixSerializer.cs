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

        public object Deserialize(TextReader stream)
        {
            var objects = (object[])Serializer.Read(stream);

            var matrix = Array.ConvertAll<object, double[]>(objects,
                o => Array.ConvertAll<object, double>((object[])o, i => (double)i));

            Matrix m = new Matrix(matrix);

            return m;
        }

        public void Write(TextWriter stream, object o)
        {
            if (o is Matrix)
            {
                var m = o as Matrix;
                stream.Write("[");
                for (int i = 0; i < m.Rows; i++)
                {
                    if (i > 0) stream.Write(",\n ");
                    stream.Write("[");
                    for (int j = 0; j < m.Cols; j++)
                    {
                        if (j > 0) stream.Write(", ");
                        stream.Write(m[i, j].ToString("r"));
                    }
                    stream.Write("]");
                }
                stream.Write("]");
            }
        }
    }
}
