using System;
using System.Collections.Generic;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork
{
    public class NeuralNetworkGenerator : Generator
    {
        private IFunction _activation;
        public Type Activation { get; set; }
        public Layer[] Layers { get; set; }
        public void Initialize<T>(params int[] layers)
            where T : IFunction
        {
            if (!Descriptor.Label.Discrete)
                throw new InvalidOperationException("Can't do regression with this model");

            Layers = new Layer[layers.Length + 1];
            int size = Descriptor.VectorLength;

            // hidden layers
            for (int i = 0; i < layers.Length; i++)
            {
                Layers[i] = new Layer
                {
                    W = Matrix.Rand(layers[i], size),
                    b = Vector.Zeros(layers[i])
                };
                size = layers[i];
            }

            // output layers
            Layers[layers.Length] = new Layer
            {
                W = Matrix.Rand(size, Descriptor.Label.Length),
                b = Vector.Zeros(Descriptor.Label.Length)
            };

            Activation = typeof(T);
            _activation = Ject.Create<IFunction>();
        }

        public void Forward(Matrix x, IFunction activation)
        {
            var APrev = x;
            for(int i = 0; i < Layers.Length; i++)
            {
                var W = Layers[i].W;
                var b = Layers[i].b;

                Layers[i].Z = W * APrev + b;
                Layers[i].A = APrev = activation.Compute(Layers[i].Z);
            }
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            throw new NotImplementedException();
        }
    }

    public class Layer
    {
        public Matrix W { get; set; }
        public Vector b { get; set; }
        public Matrix Z { get; set; }
        public Matrix A { get; set; }

        public override string ToString()
        {
            if (W == null || b == null)
                return base.ToString();
            else
                return $"W({W.Rows}, {W.Cols}), b({b.Length}, 1)";
        }
    }
}
