using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    public partial class Vector
    {
        public static implicit operator double[](Vector v)
        {
            return v.ToArray();
        }

        public static implicit operator Vector(double[] array)
        {
            return new Vector(array);
        }

        public static implicit operator Vector(int[] array)
        {
            Vector vector = new Vector
            {
                _asMatrixRef = false,
                _vector = new double[array.Length]
            };

            for (int i = 0; i < array.Length; i++)
                vector._vector[i] = array[i];

            return vector;
        }

        public static implicit operator Vector(float[] array)
        {
            Vector vector = new Vector
            {
                _asMatrixRef = false,
                _vector = new double[array.Length]
            };

            for (int i = 0; i < array.Length; i++)
                vector._vector[i] = array[i];

            return vector;
        }

        public static bool operator ==(Vector one, Vector two)
        {
            return (object.ReferenceEquals(one, null) && object.ReferenceEquals(two, null) || one.Equals(two));
        }

        public static bool operator !=(Vector one, Vector two)
        {
            return !one.Equals(two);
        }

        public static Vector operator -(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            Vector result = one.Copy();
            for (int i = 0; i < result.Length; i++)
                result[i] -= two[i];

            return result;
        }

        public static Vector operator -(Vector v, double s)
        {
            for (int i = 0; i < v.Length; i++)
                v[i] -= s;

            return v;
        }

        public static Vector operator -(double s, Vector v)
        {
            return v - s;
        }

        public static Vector operator +(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            Vector result = one.Copy();
            for (int i = 0; i < result.Length; i++)
                result[i] += two[i];

            return result;
        }

        public static Vector operator +(Vector v, double s)
        {
            for (int i = 0; i < v.Length; i++)
                v[i] += s;

            return v;
        }

        public static Vector operator +(double s, Vector v)
        {
            return v + s;
        }

        public static Vector operator -(Vector one)
        {
            Vector result = one.Copy();
            for (int i = 0; i < result.Length; i++)
                result[i] *= -1;

            return result;
        }

        public static Vector operator *(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");
            var result = one.Copy();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two[i];
            return result;
        }

        public static Vector operator *(Vector one, double two)
        {
            Vector result = one.Copy();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two;
            return result;
        }

        public static Vector operator *(Vector one, int two)
        {
            Vector result = one.Copy();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two;
            return result;
        }

        public static Vector operator *(double one, Vector two)
        {
            return two * one;
        }

        public static Vector operator /(Vector one, double two)
        {
            Vector result = one.Copy();
            for (int i = 0; i < one.Length; i++)
                result[i] /= two;
            return result;
        }

        public static Vector operator /(Vector one, int two)
        {
            Vector result = one.Copy();
            for (int i = 0; i < one.Length; i++)
                result[i] /= two;

            return result;
        }

        public static Vector operator ^(Vector one, double power)
        {
            return one.Each(d => System.Math.Pow(d, power));
        }
    }
}
