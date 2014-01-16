using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Perceptron
{
    public class KernelPerceptronGenerator : Generator
    {
        public IKernel Kernel { get; set; }

        public KernelPerceptronGenerator(IKernel kernel)
        {
            Kernel = kernel;
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            int N = y.Length;
            Vector a = Vector.Zeros(N);

            // compute kernel
            Matrix K = Kernel.Compute(x);

            int n = 1;

            // hopefully enough to converge right? ;)
            // need to be smarter about storing SPD kernels...
            bool found_error = true;
            while (n < 500 && found_error)
            {
                found_error = false;
                for (int i = 0; i < N; i++)
                {
                    found_error = y[i] * a.Dot(K[i]) <= 0;
                    if (found_error) a[i] += y[i];
                }

                n++;
            }

            // anything that *matters*
            // i.e. support vectors
            var indices = a.Indices(d => d != 0);

            // slice up examples to contain
            // only support vectors
            return new KernelPerceptronModel
            {
                Kernel = Kernel,
                A = a.Slice(indices),
                Y = y.Slice(indices),
                X = x.Slice(indices)
            };
        }
    }
}
