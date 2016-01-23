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
        object Deserialize(StreamReader stream);
        void Serialize(StreamWriter stream, object o);
    }

    public class MatrixSerializer : ISerializer
    {
        public object Deserialize(StreamReader stream)
        {
            return null;
        }

        public void Serialize(StreamWriter stream, object o)
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
