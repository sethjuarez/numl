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

            // add output layer
            Array.Resize(ref layers, layers.Length + 1);
            layers[layers.Length - 1] = Descriptor.Label.Length;

            Layers = new Layer[layers.Length];
            int size = Descriptor.VectorLength;

            // layers
            for (int i = 0; i < layers.Length; i++)
            {
                Layers[i] = new Layer
                {
                    W = Matrix.Rand(layers[i], size),
                    b = Vector.Zeros(layers[i])
                };
                size = layers[i];
            }

            Activation = typeof(T);
        }

        public void Forward(Matrix x, IFunction activation)
        {
            var sigmoid = new Sigmoid();
            var APrev = x;
            for(int i = 0; i < Layers.Length; i++)
            {
                var Li = Layers[i];
                Layers[i].Z = Li.W * APrev.T + Li.b;
                Layers[i].A = APrev = 
                    i == Layers.Length - 1 ?
                    sigmoid.Compute(Layers[i].Z) :
                    activation.Compute(Layers[i].Z);
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
