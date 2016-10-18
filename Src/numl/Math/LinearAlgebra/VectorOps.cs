// file:	Math\LinearAlgebra\VectorOps.cs
//
// summary:	Implements the vector ops class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A vector.</summary>
    public partial class Vector
    {
        /// <summary>double[] casting operator.</summary>
        /// <param name="v">The Vector to process.</param>
        public static implicit operator double[](Vector v)
        {
            return v.ToArray();
        }
        /// <summary>Vector casting operator.</summary>
        /// <param name="array">The array.</param>
        public static implicit operator Vector(double[] array)
        {
            return new Vector(array);
        }
        /// <summary>Vector casting operator.</summary>
        /// <param name="array">The array.</param>
        public static implicit operator Vector(int[] array)
        {
            var vector = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                vector[i] = array[i];

            return new Vector(vector);
        }
        /// <summary>Vector casting operator.</summary>
        /// <param name="array">The array.</param>
        public static implicit operator Vector(float[] array)
        {
            var vector = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                vector[i] = array[i];

            return new Vector(vector);
        }
        /// <summary>Equality operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==(Vector one, Vector two)
        {
            return (ReferenceEquals(one, null) && ReferenceEquals(two, null) || one.Equals(two));
        }
        /// <summary>Inequality operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=(Vector one, Vector two)
        {
            return !one.Equals(two);
        }
        /// <summary>Subtraction operator.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator -(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            var result = one.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] -= two[i];

            return new Vector(result);
        }
        /// <summary>Subtraction operator.</summary>
        /// <param name="v">The Vector to process.</param>
        /// <param name="s">The double to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator -(Vector v, double s)
        {
            var result = v.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] -= s;

            return new Vector(result);
        }
        /// <summary>Subtraction operator.</summary>
        /// <param name="s">The double to process.</param>
        /// <param name="v">The Vector to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator -(double s, Vector v)
        {
            var result = v.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] = s - result[i];

            return new Vector(result);
        }
        /// <summary>Addition operator.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator +(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            var result = one.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] += two[i];

            return new Vector(result);
        }
        /// <summary>Addition operator.</summary>
        /// <param name="v">The Vector to process.</param>
        /// <param name="s">The double to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator +(Vector v, double s)
        {
            var result = v.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] += s;

            return new Vector(result);
        }
        /// <summary>Addition operator.</summary>
        /// <param name="s">The double to process.</param>
        /// <param name="v">The Vector to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator +(double s, Vector v)
        {
            return v + s;
        }
        /// <summary>Negation operator.</summary>
        /// <param name="one">The one.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator -(Vector one)
        {
            var result = one.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i] *= -1;

            return new Vector(result);
        }
        /// <summary>Multiplication operator.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator *(Vector one, Vector two)
        {
            if (one.Length != two.Length)
                throw new InvalidOperationException("Dimensions do not match!");

            var result = one.ToArray();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two[i];
            return new Vector(result);
        }

        /// <summary>Multiplication operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator *(Vector one, double two)
        {
            var result = one.ToArray();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two;
            return new Vector(result);
        }
        /// <summary>Multiplication operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator *(Vector one, int two)
        {
            var result = one.ToArray();
            for (int i = 0; i < one.Length; i++)
                result[i] *= two;
            return new Vector(result);
        }
        /// <summary>Multiplication operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator *(double one, Vector two)
        {
            return two * one;
        }
        /// <summary>Division operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator /(Vector one, double two)
        {
            var result = one.ToArray();
            for (int i = 0; i < one.Length; i++)
                result[i] /= two;
            return new Vector(result);
        }
        /// <summary>Division operator.</summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator /(Vector one, int two)
        {
            var result = one.ToArray();
            for (int i = 0; i < one.Length; i++)
                result[i] /= two;

            return new Vector(result);
        }
        /// <summary>
        /// Mod operator.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The mod.</param>
        /// <returns></returns>
        public static Vector operator %(Vector one, double two)
        {
            return one.Each(d => d % two);
        }
        /// <summary>Raises each value to the specified power.</summary>
        /// <param name="one">The one.</param>
        /// <param name="power">The power.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector operator ^(Vector one, double power)
        {
            return one.Each(d => System.Math.Pow(d, power));
        }

        /// <summary>
        /// Returns an array of matching indices where each value is less than the supplied value.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static IEnumerable<int> operator <(Vector one, double val)
        {
            for (int i = 0; i < one.Length; i++)
                if (one[i] < val)
                    yield return i;
        }
        /// <summary>
        /// Returns an array of matching indices where each value is greater than the supplied value.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static IEnumerable<int> operator >(Vector one, double val)
        {
            for (int i = 0; i < one.Length; i++)
                if (one[i] > val)
                    yield return i;
        }
    }
}
