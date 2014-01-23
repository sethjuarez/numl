using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Xml.Schema;
using System.Globalization;
using numl.Math.Probability;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    [XmlRoot("m"), Serializable]
    public partial class Matrix : IXmlSerializable
    {
        private double[][] _matrix;
        private bool _asTransposeRef;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        //--------------- ctor

        /// <summary>
        /// Used only internally
        /// </summary>
        private Matrix()
        {

        }

        /// <summary>
        /// Create matrix n x n matrix
        /// </summary>
        /// <param name="n">size</param>
        public Matrix(int n) :
            this(n, n)
        {

        }

        /// <summary>
        /// Create new n x d matrix
        /// </summary>
        /// <param name="n">rows</param>
        /// <param name="d">cols</param>
        public Matrix(int n, int d)
        {
            _asTransposeRef = false;
            Rows = n;
            Cols = d;
            _matrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                _matrix[i] = new double[d];
                for (int j = 0; j < d; j++)
                    _matrix[i][j] = 0;
            }

        }

        /// <summary>
        /// Create new matrix with prepopulated
        /// vals
        /// </summary>
        /// <param name="m">initial matrix</param>
        public Matrix(double[,] m)
        {
            _asTransposeRef = false;
            Rows = m.GetLength(0);
            Cols = m.GetLength(1);
            _matrix = new double[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                _matrix[i] = new double[Cols];
                for (int j = 0; j < Cols; j++)
                    _matrix[i][j] = m[i, j];
            }
        }

        public Matrix(double[][] m)
        {
            _asTransposeRef = false;
            Rows = m.GetLength(0);
            if (Rows > 0)
                Cols = m[0].Length;
            else
                throw new InvalidOperationException("Insufficient information to construct Matrix");

            _matrix = m;
        }

        //--------------- access
        /// <summary>
        /// Accessor
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        /// <returns></returns>
        public virtual double this[int i, int j]
        {
            get
            {
                if (!_asTransposeRef)
                    return _matrix[i][j];
                else
                    return _matrix[j][i];
            }
            set
            {
                if (_asTransposeRef)
                    throw new InvalidOperationException("Cannot modify matrix in read-only transpose mode!");

                _matrix[i][j] = value;
            }
        }

        /// <summary>
        /// Returns row vector specified at index i
        /// </summary>
        /// <param name="i">row index</param>
        /// <returns></returns>
        public virtual Vector this[int i]
        {
            get
            {
                return this[i, VectorType.Row];
            }
            set
            {
                this[i, VectorType.Row] = value;
            }
        }

        /// <summary>
        /// returns col/row vector at index j
        /// </summary>
        /// <param name="i">Col/Row</param>
        /// <param name="t">Row or Column</param>
        /// <returns>Vector</returns>
        public virtual Vector this[int i, VectorType t]
        {
            get
            {
                // switch it up if using a transposed version
                if (_asTransposeRef)
                    t = t == VectorType.Row ? VectorType.Col : VectorType.Row;


                if (t == VectorType.Row)
                {
                    if (i >= Rows)
                        throw new IndexOutOfRangeException();

                    return new Vector(_matrix, i, true);
                }
                else
                {
                    if (i >= Cols)
                        throw new IndexOutOfRangeException();

                    return new Vector(_matrix, i);
                }
            }
            set
            {
                if (_asTransposeRef)
                    throw new InvalidOperationException("Cannot modify matrix in read-only transpose mode!");

                if (t == VectorType.Row)
                {
                    if (i >= Rows)
                        throw new IndexOutOfRangeException();

                    if (value.Length > Cols)
                        throw new InvalidOperationException(string.Format("Vector has lenght larger then {0}", Cols));

                    for (int k = 0; k < Cols; k++)
                        _matrix[i][k] = value[k];
                }
                else
                {
                    if (i >= Cols)
                        throw new IndexOutOfRangeException();

                    if (value.Length > Rows)
                        throw new InvalidOperationException(string.Format("Vector has lenght larger then {0}", Cols));


                    for (int k = 0; k < Rows; k++)
                        _matrix[k][i] = value[k];
                }
            }
        }

        public Vector Row(int i)
        {
            return this[i, VectorType.Row];
        }

        public Vector Col(int i)
        {
            return this[i, VectorType.Col];
        }

        public double this[Func<double, bool> f]
        {
            set
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (f(_matrix[i][j]))
                            this[i, j] = value;
            }
        }

        public Matrix this[Func<Vector, bool> f, VectorType t]
        {
            get
            {
                int count = 0;
                if (t == VectorType.Row)
                {
                    for (int i = 0; i < Rows; i++)
                        if (f(this[i, t]))
                            count++;

                    Matrix m = new Matrix(count, Cols);
                    int j = -1;
                    for (int i = 0; i < Rows; i++)
                        if (f(this[i, t]))
                            m[++j, t] = this[i, t];

                    return m;
                }
                else
                {
                    for (int i = 0; i < Cols; i++)
                        if (f(this[i, t]))
                            count++;

                    Matrix m = new Matrix(Rows, count);
                    int j = -1;
                    for (int i = 0; i < Cols; i++)
                        if (f(this[i, t]))
                            m[++j, t] = this[i, t];

                    return m;
                }

            }
        }

        public Vector GetVector(int index, int from, int to, VectorType type)
        {
            double[] v = (double[])Array.CreateInstance(typeof(double), to - from + 1);
            for (int i = from, j = 0; i < to + 1; i++, j++)
                v[j] = this[index, type][i];
            return new Vector(v);
        }

        public Matrix GetMatrix(int d1, int d2, int n1, int n2)
        {
            Matrix m = Matrix.Zeros(n2 - n1 + 1, d2 - d1 + 1);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] = this[i + n1, j + d1];
            return m;
        }

        public IEnumerable<Vector> GetRows()
        {
            for (int i = 0; i < Rows; i++)
                yield return this[i, VectorType.Row];
        }

        public IEnumerable<Vector> GetCols()
        {
            for (int i = 0; i < Cols; i++)
                yield return this[i, VectorType.Col];
        }

        public Vector ToVector()
        {
            if (Rows == 1)
                return this[0, VectorType.Row].Copy();

            if (Cols == 1)
                return this[0, VectorType.Col].Copy();

            throw new InvalidOperationException("Matrix conversion failed: More then one row or one column!");
        }


        /// <summary>
        /// Returns read-only transpose (uses matrix reference
        /// to save space)
        /// It will throw an exception if there is an attempt
        /// to write to the matrix
        /// </summary>
        public Matrix T
        {
            get
            {
                return new Matrix
                {
                    _asTransposeRef = true,
                    _matrix = _matrix,
                    Cols = Rows,
                    Rows = Cols
                };
            }
        }

        /// <summary>
        /// Deep copy transpose
        /// </summary>
        /// <returns>Matrix</returns>
        public Matrix Transpose()
        {
            var m = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    m[j, i] = this[i, j];
            return m;
        }

        /// <summary>
        /// create deep copy of matrix
        /// </summary>
        /// <returns>Matrix</returns>
        public Matrix Copy()
        {
            var m = Matrix.Zeros(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    m[i, j] = this[i, j];
            return m;

        }


        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }

        public bool Equals(Matrix m, double tol)
        {
            if (Rows != m.Rows || Cols != m.Cols)
                return false;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    if (System.Math.Abs(this[i, j] - m[i, j]) > tol)
                        return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                var m = obj as Matrix;
                if (Rows != m.Rows || Cols != m.Cols)
                    return false;

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (this[i, j] != m[i, j])
                            return false;

                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            int maxlpad = int.MinValue;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    string lpart = this[i, j].ToString("F6");
                    if (lpart.Length > maxlpad)
                        maxlpad = lpart.Length;
                }
            }
            StringBuilder matrix = new StringBuilder();
            matrix.Append("\n[");
            for (int i = 0; i < Rows; i++)
            {
                if (i == 0)
                    matrix.Append("[ ");
                else
                    matrix.Append(" [ ");

                for (int j = 0; j < Cols; j++)
                {
                    matrix.Append(" ");
                    matrix.Append(this[i, j].ToString("F6", CultureInfo.InvariantCulture).PadLeft(maxlpad));
                    if (j < Cols - 1)
                        matrix.Append(",");
                }

                if (i < Rows - 1)
                    matrix.Append("],\n");
                else
                    matrix.Append("]]");
            }

            return matrix.ToString();
        }

        //--------------- creation
        /// <summary>
        /// Initial Zero Matrix (n by n)
        /// </summary>
        /// <param name="n">Size</param>
        /// <returns>Matrix</returns>
        public static Matrix Zeros(int n)
        {
            return new Matrix(n, n);
        }

        /// <summary>
        /// n x d identity matrix
        /// </summary>
        /// <param name="n">rows</param>
        /// <param name="d">cols</param>
        /// <returns>Matrix</returns>
        public static Matrix Identity(int n, int d)
        {
            var m = new double[n][];
            for (int i = 0; i < n; i++)
            {
                m[i] = new double[d];
                for (int j = 0; j < d; j++)
                    if (i == j)
                        m[i][j] = 1;
                    else
                        m[i][j] = 0;
            }

            return new Matrix
            {
                _matrix = m,
                Rows = n,
                Cols = d,
                _asTransposeRef = false
            };
        }

        /// <summary>
        /// Generate a matrix n x d with numbers
        /// 0 less than x less than 1 drawn uniformly at
        /// random
        /// </summary>
        /// <param name="n">rows</param>
        /// <param name="d">cols</param>
        /// <returns>n x d Matrix</returns>
        public static Matrix Rand(int n, int d, double min = 0)
        {
            var m = new double[n][];
            for (int i = 0; i < n; i++)
            {
                m[i] = new double[d];
                for (int j = 0; j < d; j++)
                    m[i][j] = Sampling.GetUniform() + min;
            }

            return new Matrix { _matrix = m, _asTransposeRef = false, Cols = d, Rows = n };
        }

        public static Matrix Rand(int n, double min = 0)
        {
            return Matrix.Rand(n, n, min);
        }

        public static Matrix NormRand(int n, int d, double min = 0)
        {
            var m = new double[n][];
            for (int i = 0; i < n; i++)
            {
                m[i] = new double[d];
                for (int j = 0; j < d; j++)
                    m[i][j] = Sampling.GetNormal() + min;
            }

            return new Matrix { _matrix = m, _asTransposeRef = false, Cols = d, Rows = n };
        }

        public static Matrix NormRand(int n, double min = 0)
        {
            return Matrix.NormRand(n, n, min);
        }

        public static Matrix NormRand(Vector means, Vector stdDev, int n)
        {
            if (means.Length != stdDev.Length)
                throw new InvalidOperationException("Invalid Dimensionality");

            int d = means.Length;
            var m = new double[n][];

            for (int i = 0; i < n; i++)
            {
                m[i] = new double[d];
                for (int j = 0; j < d; j++)
                    m[i][j] = Sampling.GetNormal(means[j], stdDev[j]);
            }

            return new Matrix { _matrix = m, _asTransposeRef = false, Cols = d, Rows = n };
        }

        /// <summary>
        /// Initial zero matrix
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Matrix Zeros(int n, int d)
        {
            return new Matrix(n, d);
        }

        /// <summary>
        /// n x n identity matrix
        /// </summary>
        /// <param name="n">Size</param>
        /// <returns>Matrix</returns>
        public static Matrix Identity(int n)
        {
            return Identity(n, n);
        }

        public static Matrix Create(int n, Func<double> f)
        {
            return Create(n, n, f);
        }

        public static Matrix Create(int n, int d, Func<double> f)
        {
            Matrix matrix = new Matrix(n, d);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    matrix[i, j] = f();
            return matrix;
        }

        public static Matrix Create(int n, Func<int, int, double> f)
        {
            return Create(n, n, f);
        }

        public static Matrix Create(int n, int d, Func<int, int, double> f)
        {
            Matrix matrix = new Matrix(n, d);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    matrix[i, j] = f(i, j);
            return matrix;
        }

        //--------------- aggregation/structural
        public void SwapRow(int from, int to)
        {
            Swap(from, to, VectorType.Row);
        }

        public void SwapCol(int from, int to)
        {
            Swap(from, to, VectorType.Col);
        }

        public void Swap(int from, int to, VectorType t)
        {
            var temp = this[from, t].Copy();
            this[from, t] = this[to, t];
            this[to, t] = temp;
        }

        public Matrix Remove(int index, VectorType t)
        {
            int max = t == VectorType.Row ? Rows : Cols;
            int row = t == VectorType.Row ? Rows - 1 : Rows;
            int col = t == VectorType.Col ? Cols - 1 : Cols;

            Matrix m = new Matrix(row, col);
            int j = -1;
            for (int i = 0; i < max; i++)
            {
                if (i == index) continue;
                m[++j, t] = this[i, t];
            }

            return m;
        }

        //-------------- destructive ops
        /// <summary>
        /// In place normalization.
        /// WARNING: WILL UPDATE MATRIX!
        /// </summary>
        /// <param name="t"></param>
        public void Normalize(VectorType t)
        {
            int max = t == VectorType.Row ? Rows : Cols;
            for (int i = 0; i < max; i++)
                this[i, t] /= this[i, t].Norm();
        }

        /// <summary>
        /// In place centering.
        /// WARNING: WILL UPDATE MATRIX!
        /// </summary>
        /// <param name="t"></param>
        public Matrix Center(VectorType t)
        {
            int max = t == VectorType.Row ? Rows : Cols;
            for (int i = 0; i < max; i++)
                this[i, t] -= this[i, t].Mean();
            return this;
        }


        //---------------- Xml Serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            Rows = int.Parse(reader.GetAttribute("rows"));
            Cols = int.Parse(reader.GetAttribute("cols"));

            reader.ReadStartElement();


            if (Rows > 0 && Cols > 0)
            {
                _asTransposeRef = false;
                _matrix = new double[Rows][];
                for (int i = 0; i < Rows; i++)
                {
                    reader.ReadStartElement("r");
                    _matrix[i] = new double[Cols];
                    for (int j = 0; j < Cols; j++)
                        _matrix[i][j] = double.Parse(reader.ReadElementString("e"));
                    reader.ReadEndElement();
                }
            }
            else
                throw new InvalidOperationException("Invalid matrix size in XML!");

            reader.ReadEndElement();

        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("cols", Cols.ToString());
            writer.WriteAttributeString("rows", Rows.ToString());

            for (int i = 0; i < Rows; i++)
            {
                writer.WriteStartElement("r");
                for (int j = 0; j < Cols; j++)
                {
                    writer.WriteStartElement("e");
                    writer.WriteValue(_matrix[i][j]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        public void Save(string file)
        {
            using (var stream = File.OpenWrite(file))
                Save(stream);
        }

        public void Save(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(Environment.NewLine);
                // header
                writer.Write(Rows);
                writer.Write(",");
                writer.Write(Cols);
                writer.Write(Environment.NewLine);

                // contents
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        if (j > 0) writer.Write(",");
                        writer.Write(this[i, j].ToString("r"));
                    }
                    writer.Write(Environment.NewLine);
                }
            }
        }

        public static Matrix Load(string file)
        {
            if (File.Exists(file))
                using (var stream = File.OpenRead(file)) return Load(stream);
            else
                throw new FileNotFoundException();
        }

        public static Matrix Load(Stream stream)
        {
            Matrix matrix = null;
            using (var reader = new StreamReader(stream))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (!line.StartsWith("--"))
                    {
                        var numbers = line.Split(',');
                        // need to create new matrix
                        if (matrix == null)
                        {
                            if (numbers.Length >= 2)
                                matrix = new Matrix(int.Parse(numbers[0].Trim()), int.Parse(numbers[1].Trim()));
                            else
                                throw new InvalidOperationException("Invalid matrix format");
                        }
                        else
                        {
                            for (int j = 0; j < numbers.Length; j++)
                                matrix[i, j] = double.Parse(numbers[j]);
                            i++;
                        }
                    }
                }
            }

            return matrix;
        }
    }
}
