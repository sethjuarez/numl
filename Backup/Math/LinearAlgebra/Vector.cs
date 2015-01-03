// file:	Math\LinearAlgebra\Vector.cs
//
// summary:	Implements the vector class
using System;
using System.Xml;
using System.Text;
using System.Linq;
using System.Xml.Schema;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A vector.</summary>
    [XmlRoot("v"), Serializable]
    public partial class Vector : IXmlSerializable, IEnumerable<double>
    {
        /// <summary>The vector.</summary>
        private double[] _vector;
        /// <summary>true to as matrix reference.</summary>
        private bool _asMatrixRef;
        /// <summary>true to as col.</summary>
        private readonly bool _asCol;
        /// <summary>The matrix.</summary>
        private readonly double[][] _matrix = null;
        /// <summary>Zero-based index of the static.</summary>
        private int _staticIdx = -1;
        /// <summary>The transpose.</summary>
        private Matrix _transpose;
        /// <summary>
        /// this is when the values are actually referencing a vector in an existing matrix.
        /// </summary>
        /// <param name="m">private matrix vals.</param>
        /// <param name="idx">static col reference.</param>
        /// <param name="asCol">(Optional) true to as col.</param>
        internal Vector(double[][] m, int idx, bool asCol = false)
        {
            _asCol = asCol;
            _asMatrixRef = true;
            _matrix = m;
            _staticIdx = idx;
        }
        /// <summary>
        /// Constructor that prevents a default instance of this class from being created.
        /// </summary>
        private Vector()
        {
            _asCol = false;
            _asMatrixRef = false;
        }
        /// <summary>
        /// this is when the values are actually referencing a vector in an existing matrix.
        /// </summary>
        /// <param name="n">The int to process.</param>
        public Vector(int n)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = new double[n];
            for (int i = 0; i < n; i++)
                _vector[i] = 0;
        }
        /// <summary>
        /// this is when the values are actually referencing a vector in an existing matrix.
        /// </summary>
        /// <param name="contents">The contents.</param>
        public Vector(IEnumerable<double> contents)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = contents.ToArray();
        }
        /// <summary>
        /// this is when the values are actually referencing a vector in an existing matrix.
        /// </summary>
        /// <param name="contents">The contents.</param>
        public Vector(double[] contents)
        {
            _asCol = false;
            _asMatrixRef = false;
            _vector = contents;
        }
        /// <summary>Indexer to set items within this collection using array index syntax.</summary>
        /// <param name="f">The Predicate&lt;double&gt; to process.</param>
        /// <returns>The indexed item.</returns>
        public double this[Predicate<double> f]
        {
            set
            {
                for (int i = 0; i < Length; i++)
                    if (f(this[i]))
                        this[i] = value;
            }
        }
        /// <summary>Indexer to set items within this collection using array index syntax.</summary>
        /// <param name="slice">The slice.</param>
        /// <returns>The indexed item.</returns>
        public double this[IEnumerable<int> slice]
        {
            set
            {
                foreach (int i in slice)
                    this[i] = value;
            }
        }
        /// <summary>
        /// Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        /// <param name="i">Zero-based index of the entry to access.</param>
        /// <returns>The indexed item.</returns>
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
        /// <summary>Gets the length.</summary>
        /// <value>The length.</value>
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
        /// <summary>Gets the t.</summary>
        /// <value>The t.</value>
        public Matrix T
        {
            get
            {
                if (_transpose == null)
                    _transpose = new Matrix(Length, 1);
                _transpose[0, VectorType.Col] = this;
                return _transpose;
            }
        }
        /// <summary>Copies this object.</summary>
        /// <returns>A Vector.</returns>
        public Vector Copy()
        {
            var v = new Vector(Length);
            for (int i = 0; i < Length; i++)
                v[i] = this[i];
            return v;
        }
        /// <summary>Convert this object into an array representation.</summary>
        /// <returns>An array that represents the data in this object.</returns>
        public double[] ToArray()
        {
            double[] toReturn = new double[Length];
            for (int i = 0; i < Length; i++)
                toReturn[i] = this[i];
            return toReturn;
        }
        /// <summary>Converts a t to a matrix.</summary>
        /// <returns>t as a Matrix.</returns>
        public Matrix ToMatrix()
        {
            return ToMatrix(VectorType.Row);
        }
        /// <summary>Converts a t to a matrix.</summary>
        /// <param name="t">The VectorType to process.</param>
        /// <returns>t as a Matrix.</returns>
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
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
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
        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode()
        {
            if (_asMatrixRef)
                return _matrix.GetHashCode();
            else
                return _vector.GetHashCode();
        }
        /// <summary>Maximum index.</summary>
        /// <param name="startAt">The start at.</param>
        /// <returns>An int.</returns>
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
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable
        /// interface, you should return null (Nothing in Visual Basic) from this method, and instead, if
        /// specifying a custom schema is required, apply the
        /// <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the
        /// object that is produced by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" />
        /// method and consumed by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" />
        /// method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
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
        /// <summary>Converts an object into its XML representation.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
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

        /// <summary>The empty.</summary>
        public static readonly Vector Empty = new[] { 0 };
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
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
        /// <summary>Creates a new Vector.</summary>
        /// <param name="length">The length.</param>
        /// <param name="f">The Func&lt;int,double&gt; to process.</param>
        /// <returns>A Vector.</returns>
        public static Vector Create(int length, Func<double> f)
        {
            double[] vector = new double[length];
            for (int i = 0; i < length; i++)
                vector[i] = f();
            return new Vector(vector);
        }
        /// <summary>Creates a new Vector.</summary>
        /// <param name="length">The length.</param>
        /// <param name="f">The Func&lt;int,double&gt; to process.</param>
        /// <returns>A Vector.</returns>
        public static Vector Create(int length, Func<int, double> f)
        {
            double[] vector = new double[length];
            for (int i = 0; i < length; i++)
                vector[i] = f(i);
            return new Vector(vector);
        }
        /// <summary>Ranges.</summary>
        /// <param name="s">The int to process.</param>
        /// <param name="e">(Optional) the int to process.</param>
        /// <returns>A Vector.</returns>
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
        /// <summary>Gets the enumerator.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }
        /// <summary>Gets the enumerator.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }
    }
}
