using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization
{
    public interface ISerializer
    {
        object Deserialize(TextReader stream);
        void Serialize(TextWriter stream, object o);
    }

    public class MatrixSerializer : ISerializer
    {
        public object Deserialize(TextReader stream)
        {
            var objects = (object[])Serializer.Parse(stream);
            
            var matrix = Array.ConvertAll<object, double[]>(objects, 
                o => Array.ConvertAll<object, double>((object[])o, i => (double)i));

            Matrix m = new Matrix(matrix);
            
            return m;
        }

        public void Serialize(TextWriter stream, object o)
        {
            if(o is Matrix)
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
