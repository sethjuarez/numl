using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    public static class VectorExtensions
    {
        public static double Sum(this Vector v)
        {
            return Vector.Sum(v);
        }

        public static double Prod(this Vector v)
        {
            return Vector.Prod(v);
        }

        public static Matrix Outer(this Vector x, Vector y)
        {
            return Vector.Outer(x, y);
        }

        public static Vector Each(this Vector v, Func<double, double> transform, bool asCopy = false)
        {
            Vector vector = v;
            if (asCopy)
                vector = v.Copy();

            for (int i = 0; i < vector.Length; i++)
                vector[i] = transform(vector[i]);

            return v;
        }

        public static IEnumerable<double> Reverse(this Vector v)
        {
            for (int i = v.Length - 1; i > -1; i--)
                yield return v[i];
        }

        public static Vector ToVector(this double[] array)
        {
            return new Vector(array);
        }

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

        public static Matrix ToMatrix(this IEnumerable<double[]> e)
        {
            return new Matrix(e.ToArray());
        }

        public static Matrix Diag(this Vector v)
        {
            return Vector.Diag(v);
        }

        public static Matrix Diag(this Vector v, int n, int d)
        {
            return Vector.Diag(v, n, d);
        }

        public static double Norm(this Vector v)
        {
            return Vector.Norm(v, 2);
        }

        public static double Norm(this Vector v, double p)
        {
            return Vector.Norm(v, p);
        }

        public static Vector Sum(this IEnumerable<Vector> source)
        {
            return source.Aggregate((s, n) => s += n);
        }

        public static Vector Mean(this IEnumerable<Vector> source)
        {
            var c = source.Count();
            if (c == 0)
                throw new InvalidOperationException("Cannot compute average of an empty set.");

            return source.Sum() / c;
        }

        public static double Mean(this Vector source)
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Cannot compute average of an empty vector.");

            return source.Sum() / source.Length;
        }

        public static double StdDev(this Vector source)
        {
            return System.Math.Sqrt(source.Variance());
        }

        public static double Variance(this Vector x)
        {
            var mean = x.Mean();
            var sum = 0d;
            for (int i = 0; i < x.Length; i++)
                sum += System.Math.Pow(x[i] - mean, 2);

            return sum / (x.Length - 1);
        }

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

        public static double Correlation(this Vector x, Vector y)
        {
            var s = (x.StdDev() * y.StdDev());
            if (s == 0) return double.NaN;
            else return x.Covariance(y) / s;
        }

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

        public static Matrix Stats(this Vector x)
        {
            return (from i in x.Distinct().OrderBy(d => d)
                    let q = (from j in x
                             where j == i
                             select j).Count()
                    select new[] { i, q, q / (double)x.Length })
                     .ToMatrix();
        }

        public static Vector Expand(this Vector source, int n)
        {
            return Vector.Combine(source, new Vector(n));
        }

        public static Vector Expand(this Vector source, Vector s)
        {
            return Vector.Combine(source, s);
        }

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

        public static Vector Round(this Vector v, int decimals = 0)
        {
            return Vector.Round(v, decimals);
        }

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

        public static IEnumerable<double> Slice(this IEnumerable<double> x, Func<double, bool> where)
        {
            foreach (double d in x)
                if (where(d))
                    yield return d;
        }

        public static double Dot(this Vector v, Vector x)
        {
            return Vector.Dot(v, x);
        }

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
        
        public static IEnumerable<double> Generate(this IEnumerable<double> seq, Func<double, double> gen)
        {
            foreach (double item in seq)
                yield return gen(item);
        }

        public static Vector ToVector(this IEnumerable<double> seq)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (double item in seq)
                v[++i] = item;
            return v;
        }

        public static Vector ToVector<T>(this IEnumerable<T> seq, Func<T, double> f)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (T item in seq)
                v[++i] = f(item);
            return v;
        }

        public static Vector ToVector(this IEnumerable<int> seq)
        {
            Vector v = Vector.Zeros(seq.Count());
            int i = -1;
            foreach (double item in seq)
                v[++i] = (double)item;
            return v;
        }

        public static int First(this Vector v, Func<int, double, bool> predicate)
        {
            for (int i = 0; i < v.Length; i++)
            {
                if (predicate(i, v[i]))
                    return i;
            }

            return -1;
        }

        public static int Last(this Vector v, Func<int, double, bool> predicate)
        {
            for (int i = v.Length - 1; i >= 0; i--)
            {
                if (predicate(i, v[i]))
                    return i;
            }

            return -1;
        }

        public static bool ContainsNaN(this Vector vector)
        {
            return Vector.ContainsNaN(vector);
        }

        public static bool IsNaN(this Vector vector)
        {
            return Vector.IsNaN(vector);
        }
    }
}
