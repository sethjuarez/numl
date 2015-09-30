// file:	Math\LinearAlgebra\VectorExtensions.cs
//
// summary:	Implements the vector extensions class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A vector extensions.</summary>
    public static class VectorExtensions
    {
        /// <summary>A Vector extension method that calcs.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="f">The Func&lt;T,double&gt; to process.</param>
        /// <returns>A Vector.</returns>
        public static Vector Calc(this Vector v, Func<double, double> f)
        {
            return Vector.Calc(v, f);
        }
        /// <summary>An IEnumerable&lt;Vector&gt; extension method that sums the given source.</summary>
        /// <param name="v">The v to act on.</param>
        /// <returns>A Vector.</returns>
        public static double Sum(this Vector v)
        {
            return Vector.Sum(v);
        }

        /// <summary>
        /// Returns the Log of the current Vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Log(this Vector v)
        {
            return Vector.Log(v);
        }
        /// <summary>A Vector extension method that products the given v.</summary>
        /// <param name="v">The v to act on.</param>
        /// <returns>A double.</returns>
        public static double Prod(this Vector v)
        {
            return Vector.Prod(v);
        }
        /// <summary>A Vector extension method that outers.</summary>
        /// <param name="x">The x to act on.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Outer(this Vector x, Vector y)
        {
            return Vector.Outer(x, y);
        }
        /// <summary>A Vector extension method that eaches.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="asCopy">(Optional) true to as copy.</param>
        /// <returns>A Vector.</returns>
        public static Vector Each(this Vector v, Func<double, double> transform, bool asCopy = false)
        {
            Vector vector = v;
            if (asCopy)
                vector = v.Copy();

            for (int i = 0; i < vector.Length; i++)
                vector[i] = transform(vector[i]);

            return v;
        }
        /// <summary>Enumerates reverse in this collection.</summary>
        /// <param name="v">The v to act on.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process reverse in this collection.
        /// </returns>
        public static IEnumerable<double> Reverse(this Vector v)
        {
            for (int i = v.Length - 1; i > -1; i--)
                yield return v[i];
        }
        /// <summary>
        /// An IEnumerable&lt;int&gt; extension method that converts a seq to a vector.
        /// </summary>
        /// <param name="array">The array to act on.</param>
        /// <returns>seq as a Vector.</returns>
        public static Vector ToVector(this double[] array)
        {
            return new Vector(array);
        }
        /// <summary>
        /// An IEnumerable&lt;double[]&gt; extension method that converts an e to a matrix.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="source">The source to act on.</param>
        /// <returns>e as a Matrix.</returns>
        public static Matrix ToMatrix(this IEnumerable<Vector> source)
        {
            var c = source.Count();
            if (c == 0)
                throw new InvalidOperationException("Cannot create matrix from an empty set.");

            var i = 0;
            double[][] m = new double[c][];
            foreach (var v in source)
            {
                m[i] = new double[v.Length];
                for (int j = 0; j < v.Length; j++)
                    m[i][j] = v[j];
                i++;
            }

            return new Matrix(m);
        }
        /// <summary>
        /// An IEnumerable&lt;double[]&gt; extension method that converts an e to a matrix.
        /// </summary>
        /// <param name="e">The e to act on.</param>
        /// <returns>e as a Matrix.</returns>
        public static Matrix ToMatrix(this IEnumerable<double[]> e)
        {
            return new Matrix(e.ToArray());
        }
        /// <summary>A Vector extension method that diags.</summary>
        /// <param name="v">The v to act on.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Diag(this Vector v)
        {
            return Vector.Diag(v);
        }
        /// <summary>A Vector extension method that diags.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="n">The int to process.</param>
        /// <param name="d">The int to process.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Diag(this Vector v, int n, int d)
        {
            return Vector.Diag(v, n, d);
        }
        /// <summary>A Vector extension method that normals.</summary>
        /// <param name="v">The v to act on.</param>
        /// <returns>A double.</returns>
        public static double Norm(this Vector v)
        {
            return Vector.Norm(v, 2);
        }
        /// <summary>A Vector extension method that normals.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="p">The double to process.</param>
        /// <returns>A double.</returns>
        public static double Norm(this Vector v, double p)
        {
            return Vector.Norm(v, p);
        }
        /// <summary>
        /// An IEnumerable&lt;Vector&gt; extension method that sums the given source.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>A Vector.</returns>
        public static Vector Sum(this IEnumerable<Vector> source)
        {
            return source.Aggregate((s, n) => s += n);
        }
        /// <summary>
        /// A Vector extension method that determines the mean of the given parameters.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="source">The source to act on.</param>
        /// <returns>The mean value.</returns>
        public static Vector Mean(this IEnumerable<Vector> source)
        {
            var c = source.Count();
            if (c == 0)
                throw new InvalidOperationException("Cannot compute average of an empty set.");

            return source.Sum() / c;
        }
        /// <summary>
        /// A Vector extension method that determines the mean of the given parameters.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="source">The source to act on.</param>
        /// <returns>The mean value.</returns>
        public static double Mean(this Vector source)
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Cannot compute average of an empty vector.");

            return source.Sum() / source.Length;
        }
        /// <summary>A Vector extension method that computes the standard deviation.</summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>A double.</returns>
        public static double StdDev(this Vector source)
        {
            return System.Math.Sqrt(source.Variance());
        }
        /// <summary>A Vector extension method that variances the given x coordinate.</summary>
        /// <param name="x">The x to act on.</param>
        /// <returns>A double.</returns>
        public static double Variance(this Vector x)
        {
            var mean = x.Mean();
            var sum = 0d;
            for (int i = 0; i < x.Length; i++)
                sum += System.Math.Pow(x[i] - mean, 2);

            return sum / (x.Length - 1);
        }
        /// <summary>A Vector extension method that covariances.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The x to act on.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public static double Covariance(this Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Vectors must be the same length.");

            var xmean = x.Mean();
            var ymean = y.Mean();

            var sum = 0d;
            for (int i = 0; i < x.Length; i++)
                sum += (x[i] - xmean) * (y[i] - ymean);

            return sum / (x.Length - 1);
        }
        /// <summary>A Vector extension method that correlations.</summary>
        /// <param name="x">The x to act on.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public static double Correlation(this Vector x, Vector y)
        {
            var s = (x.StdDev() * y.StdDev());
            if (s == 0) return double.NaN;
            else return x.Covariance(y) / s;
        }
        /// <summary>A Vector extension method that modes the given source.</summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>A double.</returns>
        public static double Mode(this Vector source)
        {
            var q = from i in source
                    group i by i into g
                    select new
                    {
                        key = g.Key,
                        count = source.Where(d => d == g.Key).Count()
                    };

            double mode = -1;
            var count = -1;
            foreach (var item in q)
            {
                if (item.count > count)
                {
                    count = item.count;
                    mode = item.key;
                }
            }

            return mode;
        }
        /// <summary>A Vector extension method that statistics the given x coordinate.</summary>
        /// <param name="x">The x to act on.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Stats(this Vector x)
        {
            return (from i in x.Distinct().OrderBy(d => d)
                    let q = (from j in x
                             where j == i
                             select j).Count()
                    select new[] { i, q, q / (double)x.Length })
                     .ToMatrix();
        }

        /// <summary>
        /// Inserts the supplied value into a new Vector at the specified position
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index">Row or Column index</param>
        /// <param name="value">Value to insert</param>
        /// <param name="insertAfter">True to add to the end, if the index matches last column</param>
        /// <returns></returns>
        public static Vector Insert(this Vector source, int index, double value, bool insertAfter = true)
        {
            var temp = source.ToList();

            if (insertAfter && (index == source.Length - 1))
                temp.Add(value);
            else
                temp.Insert(index, value);

            return new Vector(temp);
        }

        /// <summary>A Vector extension method that expands.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="n">The int to process.</param>
        /// <returns>A Vector.</returns>
        public static Vector Expand(this Vector source, int n)
        {
            return Vector.Combine(source, new Vector(n));
        }
        /// <summary>A Vector extension method that expands.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="s">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public static Vector Expand(this Vector source, Vector s)
        {
            return Vector.Combine(source, s);
        }
        /// <summary>An IEnumerable&lt;double&gt; extension method that minimum index.</summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>An int.</returns>
        public static int MinIndex(this IEnumerable<double> source)
        {
            double minValue = double.MaxValue;
            int minIndex = -1;
            int index = -1;
            foreach (double x in source)
            {
                index++;
                if (x < minValue)
                {
                    minValue = x;
                    minIndex = index;
                }
            }

            return minIndex;
        }
        /// <summary>An IEnumerable&lt;double&gt; extension method that maximum index.</summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>An int.</returns>
        public static int MaxIndex(this IEnumerable<double> source)
        {
            double maxValue = double.MinValue;
            int maxIndex = -1;
            int index = -1;
            foreach (double x in source)
            {
                index++;
                if (x > maxValue)
                {
                    maxValue = x;
                    maxIndex = index;
                }
            }

            return maxIndex;
        }
        /// <summary>Enumerates top in this collection.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="source">The source to act on.</param>
        /// <param name="n">The int to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process top in this collection.
        /// </returns>
        public static IEnumerable<int> Top(this Vector source, int n)
        {
            if (source.Length < n)
                throw new InvalidOperationException(string.Format("Cannot get top {0} from a {1} length vector", n, source.Length));

            Dictionary<int, double> hash = new Dictionary<int, double>(n);
            for (int i = 0; i < source.Length; i++)
            {
                if (hash.Count < n)
                    hash.Add(i, source[i]);
                else
                {
                    if (hash.Values.Any(d => source[i] > d))
                    {
                        var idx = (from k in hash.Keys
                                   orderby hash[k] ascending
                                   select k).First();
                        hash.Remove(idx);
                        hash.Add(i, source[i]);
                    }
                }
            }

            return hash.Keys.OrderBy(i => i);
        }
        /// <summary>Enumerates top reverse in this collection.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="source">The source to act on.</param>
        /// <param name="n">The int to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process top reverse in this collection.
        /// </returns>
        public static IEnumerable<int> TopReverse(this Vector source, int n)
        {
            if (source.Length < n)
                throw new InvalidOperationException(string.Format("Cannot get top {0} from a {1} length vector", n, source.Length));

            Dictionary<int, double> hash = new Dictionary<int, double>(n);
            for (int i = source.Length - 1; i > -1; i--)
            {
                if (hash.Count < n)
                    hash.Add(i, source[i]);
                else
                {
                    if (hash.Values.Any(d => source[i] > d))
                    {
                        var idx = (from k in hash.Keys
                                   orderby hash[k] ascending
                                   select k).First();

                        hash.Remove(idx);
                        hash.Add(i, source[i]);
                    }
                }
            }

            return hash.Keys.OrderByDescending(i => i);
        }
        /// <summary>Enumerates indices in this collection.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="f">The Func&lt;T,double&gt; to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process indices in this collection.
        /// </returns>
        public static IEnumerable<int> Indices(this IEnumerable<double> source, Func<double, bool> f)
        {
            int i = -1;
            foreach (var item in source)
            {
                ++i;
                if (f(item))
                    yield return i;
            }
        }
        /// <summary>A Vector extension method that rounds.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="decimals">(Optional) the decimals.</param>
        /// <returns>A Vector.</returns>
        public static Vector Round(this Vector v, int decimals = 0)
        {
            return Vector.Round(v, decimals);
        }
        /// <summary>Enumerates slice in this collection.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="indices">The indices.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process slice in this collection.
        /// </returns>
        public static Vector Slice(this Vector v, IEnumerable<int> indices)
        {
            var q = indices.Distinct().Where(j => j < v.Length);
            var n = q.Count();

            Vector vector = new Vector(n);
            int i = -1;
            foreach (var j in q.OrderBy(k => k))
                vector[++i] = v[j];

            return vector;
        }
        /// <summary>Enumerates slice in this collection.</summary>
        /// <param name="x">The x to act on.</param>
        /// <param name="where">The where.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process slice in this collection.
        /// </returns>
        public static IEnumerable<double> Slice(this IEnumerable<double> x, Func<double, bool> where)
        {
            foreach (double d in x)
                if (where(d))
                    yield return d;
        }
        /// <summary>A Vector extension method that dots.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="x">The x to act on.</param>
        /// <returns>A double.</returns>
        public static double Dot(this Vector v, Vector x)
        {
            return Vector.Dot(v, x);
        }
        /// <summary>A Vector extension method that segments.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The x to act on.</param>
        /// <param name="segments">The segments.</param>
        /// <returns>A Range[].</returns>
        public static Range[] Segment(this Vector x, int segments)
        {
            if (segments < 2)
                throw new InvalidOperationException("Invalid Segment Length, must > 1");

            // list max and min
            var max = x.Max();
            var min = x.Min();

            // range size
            var range = (max - min) / segments;

            // create range array
            var ranges = new Range[segments];

            // first element starts with min
            ranges[0] = Range.Make(min, min + range);

            // each subsequent element is max
            // of previous and max of previous
            // plus the appropriate range
            for (int i = 1; i < segments; i++)
                ranges[i] = Range.Make(ranges[i - 1].Max, ranges[i - 1].Max + range);

            // make last range slightly larger 
            // to maintain r.Min <= d < r.Max
            ranges[ranges.Length - 1].Max += .01;

            return ranges;
        }
        /// <summary>Enumerates generate in this collection.</summary>
        /// <param name="seq">The seq to act on.</param>
        /// <param name="gen">The generate.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process generate in this collection.
        /// </returns>
        public static IEnumerable<double> Generate(this IEnumerable<double> seq, Func<double, double> gen)
        {
            foreach (double item in seq)
                yield return gen(item);
        }
        /// <summary>
        /// An IEnumerable&lt;int&gt; extension method that converts a seq to a vector.
        /// </summary>
        /// <param name="seq">The seq to act on.</param>
        /// <returns>seq as a Vector.</returns>
        public static Vector ToVector(this IEnumerable<double> seq)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (double item in seq)
                v[++i] = item;
            return v;
        }
        /// <summary>
        /// An IEnumerable&lt;T&gt; extension method that converts this object to a vector.
        /// </summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="seq">The seq to act on.</param>
        /// <param name="f">The Func&lt;T,double&gt; to process.</param>
        /// <returns>The given data converted to a Vector.</returns>
        public static Vector ToVector<T>(this IEnumerable<T> seq, Func<T, double> f)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (T item in seq)
                v[++i] = f(item);
            return v;
        }
        /// <summary>
        /// An IEnumerable&lt;int&gt; extension method that converts a seq to a vector.
        /// </summary>
        /// <param name="seq">The seq to act on.</param>
        /// <returns>seq as a Vector.</returns>
        public static Vector ToVector(this IEnumerable<int> seq)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (double item in seq)
                v[++i] = (double)item;
            return v;
        }

        /// <summary>
        /// Return the result of a 1 x m * m x 1 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double ToDouble(this Vector vector)
        {
            return vector.First();
        }

        /// <summary>A Vector extension method that firsts.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An int.</returns>
        public static int First(this Vector v, Func<int, double, bool> predicate)
        {
            for (int i = 0; i < v.Length; i++)
            {
                if (predicate(i, v[i]))
                    return i;
            }

            return -1;
        }
        /// <summary>A Vector extension method that lasts.</summary>
        /// <param name="v">The v to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An int.</returns>
        public static int Last(this Vector v, Func<int, double, bool> predicate)
        {
            for (int i = v.Length - 1; i >= 0; i--)
            {
                if (predicate(i, v[i]))
                    return i;
            }

            return -1;
        }
        /// <summary>A Vector extension method that query if 'vector' contains na n.</summary>
        /// <param name="vector">The vector to act on.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsNaN(this Vector vector)
        {
            return Vector.ContainsNaN(vector);
        }
        /// <summary>A Vector extension method that query if 'vector' is na n.</summary>
        /// <param name="vector">The vector to act on.</param>
        /// <returns>true if na n, false if not.</returns>
        public static bool IsNaN(this Vector vector)
        {
            return Vector.IsNaN(vector);
        }
    }
}
