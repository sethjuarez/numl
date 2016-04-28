using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using numl.Supervised.Perceptron;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Serialization.Supervised
{
    public class KernelPerceptronSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(KernelPerceptronModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new KernelPerceptronModel();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var d = base.Read(reader) as KernelPerceptronModel;
                d.Kernel = reader.ReadProperty().Value as IKernel;
                d.Y = reader.ReadVectorProperty().Value as Vector;
                d.A = reader.ReadVectorProperty().Value as Vector;
                d.X = reader.ReadMatrixProperty().Value as Matrix;
                return d;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var m = value as KernelPerceptronModel;
                base.Write(writer, m);
                writer.WriteProperty(nameof(m.Kernel), m.Kernel);
                writer.WriteProperty(nameof(m.Y), m.Y);
                writer.WriteProperty(nameof(m.A), m.A);
                writer.WriteProperty(nameof(m.X), m.X);
            }
        }
    }

    public class LinearKernelSerializer : JsonSerializer<LinearKernel> { }
    public class LogisticKernelSerializer : JsonSerializer<LogisticKernel> { }
    public class PolyKernelSerializer : JsonSerializer<PolyKernel> { }
    public class RBFKernelSerializer : JsonSerializer<RBFKernel> { }

}
