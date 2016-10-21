// file:	Math\LinearAlgebra\Matrix.cs
//
// summary:	Implements the matrix class
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using numl.Math.Probability;
using System.Collections.Generic;
using numl.Utils;
using numl.Serialization;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A matrix.</summary>
    public partial class Matrix
    {
        /// <summary>The matrix.</summary>
        private double[][] _matrix;

        /// <summary>true to as transpose reference.</summary>
        private bool _asTransposeRef;

        /// <summary>Gets or sets the rows.</summary>
        /// <value>The rows.</value>
        public int Rows { get; private set; }

        /// <summary>Gets or sets the cols.</summary>
        /// <value>The cols.</value>
        public int Cols { get; private set; }

        //--------------- ctor

        /// <summary>Used only internally.</summary>
        private Matrix()
        {

        }
        /// <summary>Create matrix n x n matrix.</summary>
        /// <param name="n">size.</param>
        public Matrix(int n) :
            this(n, n)
        {

        }
        /// <summary>Create new n x d matrix.</summary>
        /// <param name="n">rows.</param>
        /// <param name="d">cols.</param>
        public Matrix(int n, int d)
        {
            _asTransposeRef = false;
            Rows = n;
            Cols = d;
            _matrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                _matrix[i] = new double[d];
            }
        }
        /// <summary>Create new matrix with prepopulated vals.</summary>
        /// <param name="m">initial matrix.</param>
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
        /// <summary>Create matrix n x n matrix.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="m">initial matrix.</param>
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
        /// <summary>Accessor.</summary>
        /// <param name="i">Row.</param>
        /// <param name="j">Column.</param>
        /// <returns>The indexed item.</returns>
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
        /// <summary>Returns row vector specified at index i.</summary>
        /// <param name="i">row index.</param>
        /// <returns>The indexed item.</returns>
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
        /// <summary>returns col/row vector at index j.</summary>
        /// <param name="i">Col/Row.</param>
        /// <param name="t">Row or Column.</param>
        /// <returns>Vector.</returns>
        public virtual Vector this[int i, VectorType t]
        {
            get
            {
                var dim = t == VectorType.Row ? Rows : Cols;
                if (i >= dim || i < 0)
                    throw new IndexOutOfRangeException();

                if (t == VectorType.Row)
                {
                    if (_asTransposeRef)
                        return new Vector(_matrix, i);
                    else
                        return new Vector(_matrix[i].ToArray());
                }
                else
                {
                    if (_asTransposeRef)
                        return new Vector(_matrix, i, true);
                    else
                    {
                        double[] cols = new double[Rows];
                        for (int j = 0; j < Rows; j++) cols[j] = _matrix[j][i];

                        return new Vector(cols);
                    };
                }
            }
            set
            {
                if (_asTransposeRef)
                    throw new InvalidOperationException("Cannot modify matrix in read-only transpose mode!");

                var dim1 = t == VectorType.Row ? Rows : Cols;
                var dim2 = t == VectorType.Row ? Cols : Rows;

                if (i >= dim1 || i < 0)
                    throw new IndexOutOfRangeException();

                if (value.Length > dim2)
                    throw new InvalidOperationException(string.Format("Vector has lenght larger then {0}", dim2));

                if (t == VectorType.Row)
                {
                    for (int k = 0; k < Cols; k++)
                        _matrix[i][k] = value[k];
                }
                else
                {
                    for (int k = 0; k < Rows; k++)
                        _matrix[k][i] = value[k];
                }
            }
        }
        /// <summary>Rows.</summary>
        /// <param name="i">Zero-based index of the.</param>
        /// <returns>A Vector.</returns>
        public Vector Row(int i)
        {
            return this[i, VectorType.Row];
        }
        /// <summary>Cols.</summary>
        /// <param name="i">Zero-based index of the.</param>
        /// <returns>A Vector.</returns>
        public Vector Col(int i)
        {
            return this[i, VectorType.Col];
        }
        /// <summary>Indexer to set items within this collection using array index syntax.</summary>
        /// <param name="f">The Func&lt;double,bool&gt; to process.</param>
        /// <returns>The indexed item.</returns>
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
        /// <summary>
        /// Indexer to set items within this collection using an n x 2 array of indices to set.
        /// </summary>
        /// <param name="slice">An n x 2 array of indices to set.</param>
        /// <returns></returns>
        public double this[IEnumerable<int[]> slice]
        {
            set
            {
                foreach (int[] i in slice)
                    this[i[0], i[1]] = value;
            }
        }
        /// <summary>Indexer to get items within this collection using array index syntax.</summary>
        /// <param name="f">The Func&lt;Vector,bool&gt; to process.</param>
        /// <param name="t">The VectorType to process.</param>
        /// <returns>The indexed item.</returns>
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
        /// <summary>Gets a vector.</summary>
        /// <param name="index">Zero-based index of the.</param>
        /// <param name="from">Source for the.</param>
        /// <param name="to">to.</param>
        /// <param name="type">The type.</param>
        /// <returns>The vector.</returns>
        public Vector GetVector(int index, int from, int to, VectorType type)
        {
            double[] v = (double[])Array.CreateInstance(typeof(double), to - from + 1);
            for (int i = from, j = 0; i < to + 1; i++, j++)
                v[j] = this[index, type][i];
            return new Vector(v);
        }
        /// <summary>Gets a matrix.</summary>
        /// <param name="d1">The first int.</param>
        /// <param name="d2">The second int.</param>
        /// <param name="n1">The first int.</param>
        /// <param name="n2">The second int.</param>
        /// <returns>The matrix.</returns>
        public Matrix GetMatrix(int d1, int d2, int n1, int n2)
        {
            Matrix m = Zeros(n2 - n1 + 1, d2 - d1 + 1);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] = this[i + n1, j + d1];
            return m;
        }
        /// <summary>Gets the rows in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the rows in this collection.
        /// </returns>
        public IEnumerable<Vector> GetRows()
        {
            for (int i = 0; i < Rows; i++)
                yield return this[i, VectorType.Row];
        }
        /// <summary>Gets the cols in this collection.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the cols in this collection.
        /// </returns>
        public IEnumerable<Vector> GetCols()
        {
            for (int i = 0; i < Cols; i++)
                yield return this[i, VectorType.Col];
        }
        /// <summary>Converts this object to a vector.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <returns>This object as a Vector.</returns>
        public Vector ToVector()
        {
            if (Rows == 1)
                return this[0, VectorType.Row].Copy();

            if (Cols == 1)
                return this[0, VectorType.Col].Copy();

            throw new InvalidOperationException("Matrix conversion failed: More then one row or one column!");
        }
        /// <summary>
        /// Returns read-only transpose (uses matrix reference to save space)
        /// It will throw an exception if there is an attempt to write to the matrix.
        /// </summary>
        /// <value>The t.</value>
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
        /// <summary>Deep copy transpose.</summary>
        /// <returns>Matrix.</returns>
        public Matrix Transpose()
        {
            return ToTransposeArray();
        }
        /// <summary>create deep copy of matrix.</summary>
        /// <returns>Matrix.</returns>
        public Matrix Copy()
        {
            return ToArray();
        }
        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="m">initial matrix.</param>
        /// <param name="tol">Double to be compared.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
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

        /// <summary>
        /// Performs a deep copy of the underlying matrix and returns a 2D array.
        /// </summary>
        /// <returns></returns>
        private double[][] ToArray()
        {
            if (_asTransposeRef)
            {
                return ToTransposeArray();
            }

            return _matrix.Select(s => s.ToArray()).ToArray();
        }

        /// <summary>
        /// Performs a deep copy of the underlying matrix, transpose it and returns a 2D array.
        /// </summary>
        /// <returns></returns>
        private double [][] ToTransposeArray()
        {
            int rows = _asTransposeRef ? Rows : Cols;
            int cols = _asTransposeRef ? Cols : Rows;
            var matrix = new double[rows][];
            for (var i = 0; i < rows; i++)
            {
                matrix[i] = new double[cols];
                for (var j = 0; j < cols; j++)
                {
                    matrix[i][j] = _matrix[j][i];
                }
            }
            return matrix;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
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
        /// <summary>Initial Zero Matrix (n by n)</summary>
        /// <param name="n">Size.</param>
        /// <returns>Matrix.</returns>
        public static Matrix Zeros(int n)
        {
            return new Matrix(n, n);
        }
        /// <summary>n x d identity matrix.</summary>
        /// <param name="n">rows.</param>
        /// <param name="d">cols.</param>
        /// <returns>Matrix.</returns>
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
        /// Generate a matrix n x d with numbers 0 less than x less than 1 drawn uniformly at random.
        /// </summary>
        /// <param name="n">rows.</param>
        /// <param name="d">cols.</param>
        /// <param name="min">(Optional) the minimum.</param>
        /// <returns>n x d Matrix.</returns>
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
        /// <summary>
        /// Generate a matrix n x d with numbers 0 less than x less than 1 drawn uniformly at random.
        /// </summary>
        /// <param name="n">rows.</param>
        /// <param name="min">(Optional) the minimum.</param>
        /// <returns>n x d Matrix.</returns>
        public static Matrix Rand(int n, double min = 0)
        {
            return Rand(n, n, min);
        }
        /// <summary>Normalise random.</summary>
        /// <param name="n">Size.</param>
        /// <param name="d">cols.</param>
        /// <param name="min">(Optional) the minimum.</param>
        /// <returns>A Matrix.</returns>
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
        /// <summary>Normalise random.</summary>
        /// <param name="n">Size.</param>
        /// <param name="min">(Optional) the minimum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix NormRand(int n, double min = 0)
        {
            return NormRand(n, n, min);
        }
        /// <summary>Normalise random.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="means">The means.</param>
        /// <param name="stdDev">The standard development.</param>
        /// <param name="n">Size.</param>
        /// <returns>A Matrix.</returns>
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
        /// <summary>Initial zero matrix.</summary>
        /// <param name="n">.</param>
        /// <param name="d">.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Zeros(int n, int d)
        {
            return new Matrix(n, d);
        }
        /// <summary>n x n identity matrix.</summary>
        /// <param name="n">Size.</param>
        /// <returns>Matrix.</returns>
        public static Matrix Identity(int n)
        {
            return Identity(n, n);
        }
        /// <summary>Creates a new Matrix.</summary>
        /// <param name="n">Size.</param>
        /// <param name="f">The Func&lt;int,int,double&gt; to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Create(int n, Func<double> f)
        {
            return Create(n, n, f);
        }
        /// <summary>Creates a new Matrix.</summary>
        /// <param name="n">Size.</param>
        /// <param name="d">cols.</param>
        /// <param name="f">The Func&lt;int,int,double&gt; to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Create(int n, int d, Func<double> f)
        {
            Matrix matrix = new Matrix(n, d);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    matrix[i, j] = f();
            return matrix;
        }
        /// <summary>Creates a new Matrix.</summary>
        /// <param name="n">Size.</param>
        /// <param name="f">The Func&lt;int,int,double&gt; to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Create(int n, Func<int, int, double> f)
        {
            return Create(n, n, f);
        }
        /// <summary>Creates a new Matrix.</summary>
        /// <param name="n">Size.</param>
        /// <param name="d">cols.</param>
        /// <param name="f">The Func&lt;int,int,double&gt; to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Create(int n, int d, Func<int, int, double> f)
        {
            Matrix matrix = new Matrix(n, d);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    matrix[i, j] = f(i, j);
            return matrix;
        }

        /// <summary>
        /// Parses a string containing MATLAB style Matrix syntax, i.e. "[[1, 2, 3]; [3, 4, 5]]"
        /// </summary>
        /// <param name="text">Input string to parse.</param>
        /// <returns>Matrix.</returns>
        public static Matrix Parse(string text)
        {
            string[] arrs = text.Split(new char[] { '[', ';', ']', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();

            int rows = arrs.Length;

            double[][] result = new double[rows][];

            for (int i = 0; i < rows; i++)
                result[i] = arrs[i].Trim().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => double.Parse(s.Trim())).ToArray();

            return new Matrix(result);
        }

        //--------------- aggregation/structural
        /// <summary>Swap row.</summary>
        /// <param name="from">Source for the.</param>
        /// <param name="to">to.</param>
        public void SwapRow(int from, int to)
        {
            Swap(from, to, VectorType.Row);
        }
        /// <summary>Swap col.</summary>
        /// <param name="from">Source for the.</param>
        /// <param name="to">to.</param>
        public void SwapCol(int from, int to)
        {
            Swap(from, to, VectorType.Col);
        }
        /// <summary>Swaps.</summary>
        /// <param name="from">Source for the.</param>
        /// <param name="to">to.</param>
        /// <param name="t">.</param>
        public void Swap(int from, int to, VectorType t)
        {
            var temp = this[from, t].Copy();
            this[from, t] = this[to, t];
            this[to, t] = temp;
        }

        /// <summary>
        /// Returns a new Matrix with the Vector inserted at the specified position
        /// </summary>
        /// <param name="v">Vector to insert</param>
        /// <param name="index">The zero based row / column.</param>
        /// <param name="t">Vector orientation</param>
        /// <param name="insertAfter">Insert after or before the last row / column</param>
        /// <returns></returns>
        public Matrix Insert(Vector v, int index, VectorType t, bool insertAfter = true)
        {
            if (t == VectorType.Col && v.Length != Rows) throw new ArgumentException("Column vector does not match matrix height");
            if (t == VectorType.Row && v.Length != Cols) throw new ArgumentException("Row vector does not match matrix width");

            if (t == VectorType.Col && (index >= Cols || index < 0) && (index != -1 || !insertAfter)) throw new ArgumentException("Column index does not match matrix width");
            if (t == VectorType.Row && (index >= Rows || index < 0) && (index != -1 || !insertAfter)) throw new ArgumentException("Row index does not match matrix height");

            var temp = ToArray().ToList();
            if (t == VectorType.Row)
            {
                if (index == temp.Count - 1 && insertAfter)
                {
                    temp.Add(v);
                }
                else
                {
                    temp.Insert(index, v);
                }
            }
            else
            {
                if (index == temp[0].Length - 1 && insertAfter)
                {
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var copy = temp[i].ToList();
                        copy.Add(v[i]);
                        temp[i] = copy.ToArray();
                    }
                }
                else
                {
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var copy = temp[i].ToList();
                        copy.Insert(index, v[i]);
                        temp[i] = copy.ToArray();
                    }
                }
            }

            return new Matrix(temp.ToArray());
        }

        /// <summary>Removes this object.</summary>
        /// <param name="index">Zero-based index of the.</param>
        /// <param name="t">.</param>
        /// <returns>A Matrix.</returns>
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
        /// <summary>In place normalization. WARNING: WILL UPDATE MATRIX!</summary>
        /// <param name="t">.</param>
        public void Normalize(VectorType t)
        {
            int max = t == VectorType.Row ? Rows : Cols;
            for (int i = 0; i < max; i++)
                this[i, t] /= this[i, t].Norm();
        }
        /// <summary>In place centering. WARNING: WILL UPDATE MATRIX!</summary>
        /// <param name="t">.</param>
        /// <returns>A Matrix.</returns>
        public Matrix Center(VectorType t)
        {
            int max = t == VectorType.Row ? Rows : Cols;
            for (int i = 0; i < max; i++)
                this[i, t] -= this[i, t].Mean();
            return this;
        }

        /// <summary>
        /// Save matrix to file
        /// </summary>
        /// <param name="file">file to save</param>
        public void Save(string file)
        {
            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
            using (var w = new JsonWriter(f))
                w.WriteMatrix(this);
        }
        /// <summary>Loads the given stream.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested file is not present.</exception>
        /// <param name="file">The file to load.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Load(string file)
        {
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open))
                using (var f = new StreamReader(fs))
                using (var r = new JsonReader(f))
                    return r.ReadMatrix();
            }
            else
                throw new InvalidOperationException("File not found");
        }
    }
}
