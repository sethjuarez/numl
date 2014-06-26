using System;
using System.Linq;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Xml;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.KNN
{
    [Serializable]
    public class KNNModel : Model
    {
        public int K { get; set; }
        public Matrix X { get; set; }
        public Vector Y { get; set; }

        public override double Predict(Vector y)
        {
            Tuple<int, double>[] distances = new Tuple<int, double>[X.Rows];

            // happens per slot so we are good to parallelize
            Parallel.For(0, X.Rows, i => distances[i] = new Tuple<int, double>(i, (y - X.Row(i)).Norm(2)));

            var slice = distances
                            .OrderBy(t => t.Item2)
                            .Take(K)
                            .Select(i => i.Item1);

            return Y.Slice(slice).Mode();
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("K", K.ToString("r"));
            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Matrix>(writer, X);
            Xml.Write<Vector>(writer, Y);

        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            K = int.Parse(reader.GetAttribute("K"));
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            X = Xml.Read<Matrix>(reader);
            Y = Xml.Read<Vector>(reader);
        }

        
    }
}
