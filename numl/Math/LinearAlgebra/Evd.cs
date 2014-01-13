using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace numl.Math.LinearAlgebra
{
    public class Evd
    {
        private Matrix A;
        private Matrix V;

        public Matrix Eigenvectors
        {
            get
            {
                return V;
            }
        }

        public Vector Eigenvalues { get; private set; }

        public Evd(Matrix a)
        {
            A = a.Copy();
            V = Matrix.Identity(A.Rows);
        }

        public double off(Matrix a)
        {
            double sum = 0;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    if (i != j)
                        sum += sqr(a[i, j]);
            return sqrt(sum);
        }

        private Tuple<double, double> schur(Matrix a, int p, int q)
        {
            double c, s = 0;
            if (a[p, q] != 0)
            {
                var tau = (a[q, q] - a[p, p]) / (2 * a[p, q]);
                var t = 0d;
                if (tau >= 0)
                    t = 1 / (tau + sqrt(tau + sqr(tau)));
                else
                    t = -1 / (-tau + sqrt(1 + sqr(tau)));

                c = 1 / sqrt(1 + sqr(t));
                s = t * c;
            }
            else
            {
                c = 1;
                s = 0;
            }

            return new Tuple<double, double>(c, s);
        }

        private void sweep(int p, int q)
        {
            // set jacobi rotation matrix
            var cs = schur(A, p, q);
            double c = cs.Item1;
            double s = cs.Item2;

            if (c != 1 || s != 0) // if rotation
            {

                /*************************
                 * perform jacobi rotation
                 *************************/
                // calculating intermediate J.T * A
                var pV = Vector.Create(A.Cols, i => A[p, i] * c + A[q, i] * -s);
                var qV = Vector.Create(A.Cols, i => A[q, i] * c + A[p, i] * s);

                // calculating A * J for inner p, q square
                var App = pV[p] * c + pV[q] * -s;
                var Apq = pV[q] * c + pV[p] * s;
                var Aqq = qV[q] * c + qV[p] * s;

                // fill in changes along box
                pV[p] = App;
                pV[q] = qV[p] = Apq;
                qV[q] = Aqq;

                /***************************
                 * store accumulated results
                 ***************************/
                var pE = Vector.Create(V.Rows, i => V[i, p] * c + V[i, q] * -s);
                var qE = Vector.Create(V.Rows, i => V[i, q] * c + V[i, p] * s);

                /****************
                 * matrix updates
                 ****************/
                // Update A
                A[p, VectorType.Col] = pV;
                A[p, VectorType.Row] = pV;
                A[q, VectorType.Col] = qV;
                A[q, VectorType.Row] = qV;

                // Update V - not critical 
                V[p, VectorType.Col] = pE;
                V[q, VectorType.Col] = qE;
            }
        }

        public void parallel()
        {
            Console.WriteLine("Starting new sweep!");
            int N = A.Cols;
            // make even pairings
            int n = N % 2 == 0 ? N : N + 1;

            // queue up round-iness of the robin
            Queue<int> queue = new Queue<int>(n - 1);

            // fill queue
            for (int i = 1; i < N; i++) queue.Enqueue(i);
            // add extra for odd pairings
            if (N % 2 == 1) queue.Enqueue(-1);

            for (int i = 0; i < n - 1; i++)
            {
                Parallel.For(0, n / 2, j =>
                {
                    int p, q, k = n - 1 - j;

                    int eK = queue.ElementAt(k - 1);
                    int eJ = j == 0 ? 0 : queue.ElementAt(j - 1);

                    p = min(eJ, eK);
                    q = max(eJ, eK);

                    // are we in a buy week?
                    if (p >= 0)
                        sweep(p, q);

                    Console.WriteLine("({0}, {1}) [{2}] {3}", p, q, Thread.CurrentThread.ManagedThreadId, p < 0 ? "buy" : "");

                });

                Console.WriteLine("----------[{0}]----------", Thread.CurrentThread.ManagedThreadId);
                // move stuff around
                queue.Enqueue(queue.Dequeue());
            }

        }

        private void factorize()
        {
            int N = A.Cols;
            for (int p = 0; p < N - 1; p++)
                for (int q = p + 1; q < N; q++)
                    sweep(p, q);
        }

        public void compute(double tol = 1.0e-10)
        {
            int s = 0;
            do
            {
                s++;
                factorize();
                // TODO: Fix parallelization
                //if (A.Cols <= 300) // small enough
                //    factorize();
                //else          // parallelize
                //    parallel();

            } while (off(A) > tol);

            sort();
        }

        private void sort()
        {
            //ordering
            var eigs = A.Diag()
                        .Select((d, i) => new Tuple<int, double>(i, d))
                        .OrderByDescending(j => j.Item2)
                        .ToArray();

            // sort eigenvectors
            var copy = V.Copy();
            for (int i = 0; i < eigs.Length; i++)
                copy[i, VectorType.Col] = V[eigs[i].Item1, VectorType.Col];

            // normalize eigenvectors
            copy.Normalize(VectorType.Col);
            V = copy;

            Eigenvalues = eigs.Select(t => t.Item2).ToArray();
        }

        #region for brevity...
        private double sqrt(double x)
        {
            return System.Math.Sqrt(x);
        }

        private double sqr(double x)
        {
            return System.Math.Pow(x, 2);
        }

        private int min(int a, int b)
        {
            return System.Math.Min(a, b);
        }

        private int max(int a, int b)
        {
            return System.Math.Max(a, b);
        }
        #endregion
    }
}
