using System;
using System.Linq;
using System.Collections.Generic;

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

        public Tuple<double, double> schur(Matrix a, int p, int q)
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

        public void compute(double tol = 1.0e-10)
        {
            int N = A.Cols;
            int sweep = 0;
            double o = off(A);

            while (off(A) > tol)
            {
                for (int p = 0; p < N - 1; p++)
                {
                    for (int q = p + 1; q < N; q++)
                    {
                        // set jacobi rotation matrix
                        var cs = schur(A, p, q);
                        double c = cs.Item1;
                        double s = cs.Item2;

                        // no work
                        if (c == 1 && s == 0) continue;

                        /*************************
                         * perform jacobi rotation
                         *************************/
                        // calculating intermediate J.T * A
                        for (int i = 0; i < A.Cols; i++)
                        {
                            var Api = A[p, i];
                            var Aqi = A[q, i];

                            A[p, i] = Api * c + Aqi * -s;
                            A[q, i] = Aqi * c + Api * s;
                        }
                        
                        // calculating A * J
                        // only inner p, q square
                        var App = A[p, p] * c + A[p, q] * -s;
                        var Apq = A[p, q] * c + A[p, p] * s;
                        var Aqq = A[q, q] * c + A[q, p] * s;

                        // col p, q is transpose of earlier calc
                        for (int i = 0; i < A.Cols; i++)
                        {
                            A[i, p] = A[p, i];
                            A[i, q] = A[q, i];
                        }

                        // fill in changes along box
                        A[p, p] = App;
                        A[q, q] = Aqq;
                        A[p, q] = A[q, p] = Apq;

                        /***************************
                         * store accumulated results
                         ***************************/
                        for (int i = 0; i < V.Rows; i++)
                        {
                            var Vip = V[i, p];
                            var Viq = V[i, q];
                            V[i, p] = Vip * c + Viq * -s;
                            V[i, q] = Viq * c + Vip * s;
                        }
                    }
                }

                sweep++;
                o = off(A);

#if DEBUG
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Sweep: {0}, off(A): {1}", sweep, o);
                Console.WriteLine("---------------------------------\n\n");
#endif
            }

            //ordering
            var eigs = A.Diag()
                        .Select((d, i) => new Tuple<int, double>(i, d))
                        .OrderByDescending(j => j.Item2)
                        .ToArray();

            var copy = V.Copy();
            for (int i = 0; i < eigs.Length; i++)
                copy[i, VectorType.Col] = V[eigs[i].Item1, VectorType.Col];

            V = copy;
            V.Normalize(VectorType.Col);
            Eigenvalues = eigs.Select(t => t.Item2).ToArray();

#if DEBUG
            Console.WriteLine("eigenvalues: ");
            Console.WriteLine(Eigenvalues);
            Console.Write("\n");
            Console.WriteLine("eigenvectors: ");
            Console.WriteLine(Eigenvectors);
            Console.Write("---------------------------------\n\n");
#endif
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
        #endregion
    }
}
