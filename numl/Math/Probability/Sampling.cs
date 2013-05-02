using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Probability
{

    /// <summary>
    /// SimpleRNG is a simple random number generator based on 
    /// George Marsaglia's MWC (multiply with carry) generator.
    /// Although it is very simple, it passes Marsaglia's DIEHARD
    /// series of random number generator tests.
    /// 
    /// Written by John D. Cook 
    /// http://www.johndcook.com
    /// </summary>
    public class Sampling
    {
        private static uint m_w;
        private static uint m_z;

        static Sampling()
        {
            // These values are not magical, just the default values Marsaglia used.
            // Any pair of unsigned integers should be fine.
            m_w = 521288629;
            m_z = 362436069;
        }

        // The random generator seed can be set three ways:
        // 1) specifying two non-zero unsigned integers
        // 2) specifying one non-zero unsigned integer and taking a default value for the second
        // 3) setting the seed from the system time

        private static void SetSeed(uint u, uint v)
        {
            if (u != 0) m_w = u;
            if (v != 0) m_z = v;
        }

        private static void SetSeed(uint u)
        {
            m_w = u;
        }

        /// <summary>
        /// Creates random number generator seed
        /// </summary>
        public static void SetSeedFromSystemTime()
        {
            DateTime dt = DateTime.Now;
            long x = dt.ToFileTime();
            SetSeed((uint)(x >> 16), (uint)(x % 4294967296));
        }

        /// <summary>
        /// Produce a uniform random sample from the open interval (0, 1).
        /// The method will not return either end point.
        /// </summary>
        /// <returns>Random Sample</returns>
        public static double GetUniform()
        {
            // 0 <= u <= 2^32
            uint u = GetUint();
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1) * 2.328306435454494e-10;
        }

        /// <summary>
        /// Produce a uniform random sample from the open interval (0, max).
        /// The method will not return either end point.
        /// </summary>
        /// <param name="max">Max (Exclusive)</param>
        /// <returns>Random Sample</returns>
        public static int GetUniform(int max)
        {
            return GetUniform(0, max);
        }

        /// <summary>
        /// Produce a uniform random sample from the open interval (min, max).
        /// The method will not return either end point.
        /// </summary>
        /// <param name="min">Min (exclusive)</param>
        /// <param name="max">Max (exclusive)</param>
        /// <returns>Random Sample</returns>
        public static int GetUniform(int min, int max)
        {
            return min + (int)(GetUniform() * ((max - min) + 1));
        }

        // This is the heart of the generator.
        // It uses George Marsaglia's MWC algorithm to produce an unsigned integer.
        // See http://www.bobwheeler.com/statistics/Password/MarsagliaPost.txt
        private static uint GetUint()
        {
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            return (m_z << 16) + (m_w & 65535);
        }

        /// <summary>
        /// Normal (Gaussian) random sample with mean 0 and standard deviation 1
        /// </summary>
        /// <returns>Random Sample</returns>
        public static double GetNormal()
        {
            // Use Box-Muller algorithm
            double u1 = GetUniform();
            double u2 = GetUniform();
            double r = System.Math.Sqrt(-2.0 * System.Math.Log(u1));
            double theta = 2.0 * System.Math.PI * u2;
            return r * System.Math.Sin(theta);
        }

        /// <summary>
        /// Normal (Gaussian) random sample with specified mean and standard deviation 
        /// </summary>
        /// <param name="mean">Mean</param>
        /// <param name="standardDeviation">Standard deviation</param>
        /// <returns>Random Sample</returns>
        public static double GetNormal(double mean, double standardDeviation)
        {
            if (standardDeviation <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                throw new ArgumentOutOfRangeException(msg);
            }
            return mean + standardDeviation * GetNormal();
        }

        /// <summary>
        /// Exponential random sample with mean 1
        /// </summary>
        /// <returns>Random Sample</returns>
        public static double GetExponential()
        {
            return -System.Math.Log(GetUniform());
        }

        /// <summary>
        /// Exponential random sample with specified mean 
        /// </summary>
        /// <param name="mean">mean parameter</param>
        /// <returns>Random Sample</returns>
        public static double GetExponential(double mean)
        {
            if (mean <= 0.0)
            {
                string msg = string.Format("Mean must be positive. Received {0}.", mean);
                throw new ArgumentOutOfRangeException(msg);
            }
            return mean * GetExponential();
        }

        public static double GetGamma(double shape, double scale)
        {
            // Implementation based on "A Simple Method for Generating Gamma Variables"
            // by George Marsaglia and Wai Wan Tsang.  ACM Transactions on Mathematical Software
            // Vol 26, No 3, September 2000, pages 363-372.

            double d, c, x, xsquared, v, u;

            if (shape >= 1.0)
            {
                d = shape - 1.0 / 3.0;
                c = 1.0 / System.Math.Sqrt(9.0 * d);
                for (; ; )
                {
                    do
                    {
                        x = GetNormal();
                        v = 1.0 + c * x;
                    }
                    while (v <= 0.0);
                    v = v * v * v;
                    u = GetUniform();
                    xsquared = x * x;
                    if (u < 1.0 - .0331 * xsquared * xsquared || System.Math.Log(u) < 0.5 * xsquared + d * (1.0 - v + System.Math.Log(v)))
                        return scale * d * v;
                }
            }
            else if (shape <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", shape);
                throw new ArgumentOutOfRangeException(msg);
            }
            else
            {
                double g = GetGamma(shape + 1.0, 1.0);
                double w = GetUniform();
                return scale * g * System.Math.Pow(w, 1.0 / shape);
            }
        }

        public static double GetChiSquare(double degreesOfFreedom)
        {
            // A chi squared distribution with n degrees of freedom
            // is a gamma distribution with shape n/2 and scale 2.
            return GetGamma(0.5 * degreesOfFreedom, 2.0);
        }

        public static double GetInverseGamma(double shape, double scale)
        {
            // If X is gamma(shape, scale) then
            // 1/Y is inverse gamma(shape, 1/scale)
            return 1.0 / GetGamma(shape, 1.0 / scale);
        }

        public static double GetWeibull(double shape, double scale)
        {
            if (shape <= 0.0 || scale <= 0.0)
            {
                string msg = string.Format("Shape and scale parameters must be positive. Recieved shape {0} and scale{1}.", shape, scale);
                throw new ArgumentOutOfRangeException(msg);
            }
            return scale * System.Math.Pow(-System.Math.Log(GetUniform()), 1.0 / shape);
        }

        public static double GetCauchy(double median, double scale)
        {
            if (scale <= 0)
            {
                string msg = string.Format("Scale must be positive. Received {0}.", scale);
                throw new ArgumentException(msg);
            }

            double p = GetUniform();

            // Apply inverse of the Cauchy distribution function to a uniform
            return median + scale * System.Math.Tan(System.Math.PI * (p - 0.5));
        }

        public static double GetStudentT(double degreesOfFreedom)
        {
            if (degreesOfFreedom <= 0)
            {
                string msg = string.Format("Degrees of freedom must be positive. Received {0}.", degreesOfFreedom);
                throw new ArgumentException(msg);
            }

            // See Seminumerical Algorithms by Knuth
            double y1 = GetNormal();
            double y2 = GetChiSquare(degreesOfFreedom);
            return y1 / System.Math.Sqrt(y2 / degreesOfFreedom);
        }

        // The Laplace distribution is also known as the double exponential distribution.
        public static double GetLaplace(double mean, double scale)
        {
            double u = GetUniform();
            return (u < 0.5) ?
                mean + scale * System.Math.Log(2.0 * u) :
                mean - scale * System.Math.Log(2 * (1 - u));
        }

        public static double GetLogNormal(double mu, double sigma)
        {
            return System.Math.Exp(GetNormal(mu, sigma));
        }

        public static double GetBeta(double a, double b)
        {
            if (a <= 0.0 || b <= 0.0)
            {
                string msg = string.Format("Beta parameters must be positive. Received {0} and {1}.", a, b);
                throw new ArgumentOutOfRangeException(msg);
            }

            // There are more efficient methods for generating beta samples.
            // However such methods are a little more efficient and much more complicated.
            // For an explanation of why the following method works, see
            // http://www.johndcook.com/distribution_chart.html#gamma_beta

            double u = GetGamma(a, 1.0);
            double v = GetGamma(b, 1.0);
            return u / (u + v);
        }

        public static double GetPower(double a, double min = 1)
        {
            double u = GetUniform();
            return min * System.Math.Pow(1d - u, -1d / (a - 1d));
        }
    }
}
