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
                    sum += i == j ? 0 : pow(A[i, j], 2);
            return sqrt(sum);
        }

        public void schur(int p, int q)
        {
            if (J == null) J = Matrix.Identity(A.Cols);
            else // reset to ident
                for (int i = 0; i < J.Rows; i++)
                    for (int j = 0; j < J.Cols; j++)
                        if (i == j)
                            J[i, j] = 1;
                        else
                            J[i, j] = 0;

            double c, s = 0;
            if (A[p, q] != 0)
            {
                var tau = (A[q, q] - A[p, p]) / (2 * A[p, q]);
                var t = 0d;
                if (tau >= 0)
                    t = 1 / (tau + sqrt(tau + pow(tau, 2)));
                else
                    t = -1 / (-tau + sqrt(1 + pow(tau, 2)));

                c = 1 / sqrt(1 + pow(t, 2));
                s = t * c;
            }
            else
            {
                c = 1;
                s = 0;
            }

            J[p, p] = c;
            J[p, q] = s;
            J[q, p] = -s;
            J[q, q] = c;

#if DEBUG
            Console.WriteLine("Jacobi Rotation:");
            Console.WriteLine(J);
#endif
            return J;
        }
        
        public void compute(double tol = .0001)
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
                        schur(p, q);
                        A = J.T * A * J;
                        V = V * J;

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
        }


        #region for brevity...
        private double sqrt(double x)
        {
            return System.Math.Sqrt(x);
        }

        private double pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }
        #endregion
    }
}
