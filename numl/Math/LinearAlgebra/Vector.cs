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
    public partial class Vector : IXmlSerializable, IEnumerable<double>
    {
        private double[] _vector;
        private bool _asMatrixRef;
        private readonly bool _asCol;
        private double[][] _matrix = null;
        private int _staticIdx = -1;
        private Matrix _transpose;

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

        public Matrix T
        {
            get
            {
                if (_transpose == null)
                    _transpose = new Matrix(Length, 1);
                _transpose[0, VectorType.Column] = this;
                return _transpose;
            }
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
