﻿using System;
using System.Linq;
using numl.Math.Probability;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    public partial class Vector
    {
        public static double Sum(Vector v)
        {
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
                sum += v[i];
            return sum;
        }

        public static Matrix Outer(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            int n = x.Length;
            Matrix m = new Matrix(n);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = x[i] * y[j];

            return m;
        }

        public static Vector Exp(Vector v)
        {
            return Calc(v, d => System.Math.Exp(d));
        }

        public static Vector Log(Vector v)
        {
            return Calc(v, d => System.Math.Log(d));
        }

        public static Vector Calc(Vector v, Func<double, double> f)
        {
            var result = v.Copy();
            for (int i = 0; i < v.Length; i++)
                result[i] = f(result[i]);
            return result;
        }

        public static Vector Calc(Vector v, Func<int, double, double> f)
        {
            var result = v.Copy();
            for (int i = 0; i < v.Length; i++)
                result[i] = f(i, result[i]);
            return result;
        }

        public static Vector Ones(int n)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
                x[i] = 1;

            return new Vector(x);
        }

        public static Vector Zeros(int n)
        {
            return new Vector(n);
        }

        public static Vector Rand(int n)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
                x[i] = Sampling.GetUniform();

            return new Vector(x);
        }

        public static Vector NormRand(int n, double mean = 0, double stdDev = 1, int precision = -1)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (precision > -1)
                    x[i] = System.Math.Round(Sampling.GetNormal(mean, stdDev), precision);
                else
                    x[i] = Sampling.GetNormal(mean, stdDev);
            }

            return new Vector(x);
        }

        public static double Dot(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            double total = 0;
            for (int i = 0; i < one.Length; i++)
                total += one[i] * two[i];
            return total;
        }

        public static double Norm(Vector x)
        {
            return Vector.Norm(x, 2);
        }

        public static double Norm(Vector x, double p)
        {
            if (p < 1) throw new InvalidOperationException("p must be greater than 0");
            double value = 0;
            if (p == 1)
            {
                for (int i = 0; i < x.Length; i++)
                    value += System.Math.Abs(x[i]);

                return value;
            }
            else if (p == int.MaxValue)
            {
                for (int i = 0; i < x.Length; i++)
                    if (System.Math.Abs(x[i]) > value)
                        value = System.Math.Abs(x[i]);
                return value;
            }
            else if (p == int.MinValue)
            {
                for (int i = 0; i < x.Length; i++)
                    if (System.Math.Abs(x[i]) < value)
                        value = System.Math.Abs(x[i]);
                return value;
            }
            else
            {
                for (int i = 0; i < x.Length; i++)
                    value += System.Math.Pow(System.Math.Abs(x[i]), p);

                return System.Math.Pow(value, 1 / p);
            }
        }

        public static Matrix Diag(Vector v)
        {
            Matrix m = Matrix.Zeros(v.Length);
            for (int i = 0; i < v.Length; i++)
                m[i, i] = v[i];
            return m;
        }

        public static Matrix Diag(Vector v, int n, int d)
        {
            Matrix m = Matrix.Zeros(n, d);
            int min = System.Math.Min(n, d);
            for (int i = 0; i < min; i++)
                m[i, i] = v[i];
            return m;
        }

        public static Vector Round(Vector v, int decimals = 0)
        {
            for (int i = 0; i < v.Length; i++)
                v[i] = System.Math.Round(v[i], decimals);
            return v;
        }

        public static Vector Combine(params Vector[] v)
        {
            if (v.Length == 0)
                throw new InvalidOperationException("Need to specify vectors to combine!");

            if (v.Length == 1)
                return v[0];

            int size = 0;
            for (int i = 0; i < v.Length; i++)
                size += v[i].Length;

            Vector r = new Vector(size);
            int z = -1;
            for (int i = 0; i < v.Length; i++)
                for (int j = 0; j < v[i].Length; j++)
                    r[++z] = v[i][j];

            return r;
        }

        public static Vector SortOrder(Vector vector)
        {
            return vector
                    .Select((d, i) => new Tuple<int, double>(i, d))
                    .OrderByDescending(t => t.Item2)
                    .Select(t => t.Item1)
                    .ToArray();
        }

    }
}
