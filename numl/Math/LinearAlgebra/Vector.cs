/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Collections;
using numl.Math.Probability;
using System.Linq;

namespace numl.Math.LinearAlgebra
{
    [XmlRoot("v"), Serializable]
    public class Vector : IXmlSerializable, IEnumerable<double>
    {
        private double[] _vector;
        private bool _asMatrixRef;
        private readonly bool _asCol;
        private double[][] _matrix = null;
        private int _staticIdx = -1;

        /// <summary>
        /// this is when the values are actually referencing
        /// a vector in an existing matrix
        /// </summary>
        /// <param name="m">private matrix vals</param>
        /// <param name="idx">static col reference</param>
        internal Vector(double[][] m, int idx, bool asCol = false)
        {
            _asCol = asCol;
            _asMatrixRef = true;
            _matrix = m;
            _staticIdx = idx;
        }

        private Vector()
        {
            _asCol = false;
            _asMatrixRef = false;
        }

        public Vector(int n)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = new double[n];
            for (int i = 0; i < n; i++)
                _vector[i] = 0;
        }

        public Vector(IEnumerable<double> contents)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = contents.ToArray();
        }

        public Vector(double[] contents)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = contents;
        }

        public double this[Predicate<double> f]
        {
            set
            {
                for (int i = 0; i < Length; i++)
                    if (f(this[i]))
                        this[i] = value;
            }
        }

        public double this[IEnumerable<int> slice]
        {
            set
            {
                foreach (int i in slice)
                    this[i] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                if (!_asMatrixRef)
                    return _vector[i];
                else
                {
                    if (_asCol)
                        return _matrix[_staticIdx][i];
                    else
                        return _matrix[i][_staticIdx];
                }
            }
            set
            {
                if (!_asMatrixRef)
                    _vector[i] = value;
                else
                {
                    if (_asCol)
                        _matrix[_staticIdx][i] = value;
                    else
                        _matrix[i][_staticIdx] = value;
                }
            }
        }

        public int Length
        {
            get
            {
                if (!_asMatrixRef)
                    return _vector.Length;
                else
                {
                    if (_asCol)
                        return _matrix[0].Length;
                    else
                        return _matrix.Length;
                }
            }
        }

        public double Sum()
        {
            double sum = 0;
            for (int i = 0; i < Length; i++)
                sum += this[i];
            return sum;
        }

        public Vector Copy()
        {
            var v = new Vector(Length);
            for (int i = 0; i < Length; i++)
                v[i] = this[i];
            return v;
        }

        public double[] ToArray()
        {
            double[] toReturn = new double[Length];
            for (int i = 0; i < Length; i++)
                toReturn[i] = this[i];
            return toReturn;
        }

        public Matrix ToMatrix()
        {
            return ToMatrix(VectorType.Row);
        }

        public Matrix ToMatrix(VectorType t)
        {
            if (t == VectorType.Row)
            {
                var m = new Matrix(1, Length);
                for (int j = 0; j < Length; j++)
                    m[0, j] = this[j];

                return m;
            }
            else
            {
                var m = new Matrix(Length, 1);
                for (int i = 0; i < Length; i++)
                    m[i, 0] = this[i];

                return m;
            }
        }

        public void Zeros()
        {
            for (int i = 0; i < Length; i++)
                this[i] = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                var m = obj as Vector;
                if (Length != m.Length)
                    return false;

                for (int i = 0; i < Length; i++)
                    if (this[i] != m[i])
                        return false;

                return true;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _vector.GetHashCode();
        }

        internal int MaxIndex(int startAt)
        {
            int idx = startAt;
            double val = 0;
            for (int i = startAt; i < Length; i++)
            {
                if (val < this[i])
                {
                    idx = i;
                    val = this[i];
                }
            }

            return idx;
        }

        public double Sum(Func<int, double, bool> f)
        {
            double total = 0;
            for (int i = 0; i < Length; i++)
                if (f(i, this[i]))
                    total += this[i];
            return total;
        }

        public double Sum(Func<int, bool> f)
        {
            double total = 0;
            for (int i = 0; i < Length; i++)
                if (f(i))
                    total += this[i];
            return total;
        }

        public double Sum(Func<double, bool> f)
        {
            double total = 0;
            for (int i = 0; i < Length; i++)
                if (f(this[i]))
                    total += this[i];
            return total;
        }
        
        //----------------- Xml Serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            int size = int.Parse(reader.GetAttribute("size"));

            reader.ReadStartElement();

            if (size > 0)
            {
                _asMatrixRef = false;
                _vector = new double[size];
                for (int i = 0; i < size; i++)
                    _vector[i] = double.Parse(reader.ReadElementString("e"));
            }
            else
                throw new InvalidOperationException("Invalid vector size in XML!");

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            if (_asMatrixRef)
                throw new InvalidOperationException("Cannot serialize a vector that is a matrix reference!");

            writer.WriteAttributeString("size", _vector.Length.ToString());
            for (int i = 0; i < _vector.Length; i++)
            {
                writer.WriteStartElement("e");
                writer.WriteValue(_vector[i]);
                writer.WriteEndElement();
            }
        }

        //----------------- Static
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

        public static double Sum(Vector v)
        {
            return v.Sum();
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
                x[i] = MLRandom.GetUniform();

            return new Vector(x);
        }

        public static Vector NormRand(int n, double mean = 0, double stdDev = 1, int precision = -1)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (precision > -1)
                    x[i] = System.Math.Round(MLRandom.GetNormal(mean, stdDev), precision);
                else
                    x[i] = MLRandom.GetNormal(mean, stdDev);
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
            else
            {
                for (int i = 0; i < x.Length; i++)
                    value += System.Math.Pow(System.Math.Abs(x[i]), p);

                return System.Math.Pow(value, 1 / p);
            }
        }

        public static double CosineSimilarity(Vector A, Vector B)
        {
            return Vector.Dot(A, B) / (A.Norm() * B.Norm());
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

        //--------------- Operations
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

        public static readonly Vector Empty = new[] { 0 };

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < Length; i++)
            {
                sb.Append(this[i].ToString("F4"));
                if (i < Length - 1)
                    sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static Vector Range(int s, int e = -1)
        {
            if (e > 0)
            {
                Vector v = Vector.Zeros(e - s);
                for (int i = s; i < e; i++)
                    v[i - s] = i;
                return v;
            }
            else
            {
                Vector v = Vector.Zeros(s);
                for (int i = 0; i < s; i++)
                    v[i] = i;
                return v;
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }
    }
}
