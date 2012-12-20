using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Math.LinearAlgebra
{
    public class Evd
    {
        private Matrix A;
        private Matrix V;
        private Matrix J;

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

        public double off(Matrix A)
        {
            double sum = 0;
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    if (i != j)
                        sum += sqr(A[i, j]);
            return sqrt(sum);
        }
        
        public Tuple<double, double> schur(int p, int q)
        {
            double c, s = 0;
            if (A[p, q] != 0)
            {
                var tau = (A[q, q] - A[p, p]) / (2 * A[p, q]);
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

        public void compute(double tol = .0001)
        {
            int N = A.Cols;
            int sweep = 0;
            double o = off(A);
            if (J == null) J = Matrix.Identity(N);

            while (off(A) > tol)
            {
                for (int p = 0; p < N - 1; p++)
                {
                    for (int q = p + 1; q < N; q++)
                    {
                        // set jacobi rotation matrix
                        var cs = schur(p, q);
                        double c = cs.Item1;
                        double s = cs.Item2;

                        // no work
                        if (c == 1 && s == 0) continue;


                        J[p, p] = J[q, q] = c;
                        J[p, q] = s; J[q, p] = -s;

#if DEBUG
                        Console.WriteLine("Jacobi Rotation:");
                        Console.WriteLine(J);
#endif
                        // perform rotation
                        A = J.T * A * J;
                        // store accumulated results
                        V = V * J;

                        // reset Jacobi
                        J[p, p] = J[q, q] = 1;
                        J[p, q] = J[q, p] = 0;
#if DEBUG
                        Console.WriteLine("eigenvalues: ");
                        Console.WriteLine(A);
                        Console.Write("\n");
                        Console.WriteLine("eigenvectors: ");
                        Console.WriteLine(V);
                        Console.Write("---------------------------------\n\n");
#endif

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
                copy[i, VectorType.Column] = V[eigs[i].Item1, VectorType.Column];

            V = copy;
            V.Normalize(VectorType.Column);
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



        public void sort(Vector v, Matrix x)
        {
            var eigs = v.Select((d, i) => new Tuple<int, double>(i, d))
                        .OrderByDescending(j => j.Item2)
                        .ToArray();
#if DEBUG
            Console.WriteLine(v);
            Console.WriteLine(x);
            foreach (var e in eigs)
                Console.Write("{0},", e);
            Console.WriteLine("");
#endif
            var copy = Matrix.Zeros(x.Rows, x.Cols);
            for (int i = 0; i < eigs.Length; i++)
                copy[i, VectorType.Column] = x[eigs[i].Item1, VectorType.Column];

            copy.Normalize(VectorType.Column);
            Vector vec = eigs.Select(t => t.Item2).ToArray();

#if DEBUG
            Console.WriteLine(vec);
            Console.WriteLine(copy);
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
