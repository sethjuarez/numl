using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Math;
using System.Globalization;

namespace numl.Tests
{
    [TestFixture]
    public class LanguageTests
    {
        [Test]
        public void Test_Assignment_Means()
        {
            var asgn = new[] { 0, 1, 0, 1, 2, 3, 0, 2, 1, 3, 1, 2 };
            var answ = (from q in asgn
                        orderby q
                        group q by q into g
                        select (double)g.Count() /
                               (double)asgn.Length)
                       .ToVector();
        }

        [Test]
        public void Test_Print_Matrix()
        {
            int n = 10;
            // two normally distributed 
            // clusters, should be good
            var x = Matrix.VStack(
                        Vector.NormRand(n, 8, 3),
                        Vector.NormRand(n, 1, 6),
                        Vector.NormRand(n, -1, 7.5),
                        Vector.NormRand(n, .5, 2));


            int maxlpad = int.MinValue;
            for (int i = 0; i < x.Rows; i++)
            {
                for (int j = 0; j < x.Cols; j++)
                {
                    string lpart = x[i, j].ToString("F6");
                    if (lpart.Length > maxlpad)
                        maxlpad = lpart.Length;
                }
            }
            StringBuilder matrix = new StringBuilder();
            matrix.Append("[");
            for (int i = 0; i < x.Rows; i++)
            {
                if (i == 0)
                    matrix.Append("[ ");
                else
                    matrix.Append(" [ ");

                for (int j = 0; j < x.Cols; j++)
                {
                    matrix.Append(" ");
                    matrix.Append(x[i, j].ToString("F6", CultureInfo.InvariantCulture).PadLeft(maxlpad));
                    if (j < x.Cols - 1)
                        matrix.Append(",");
                }

                if (i < x.Rows - 1)
                    matrix.Append("]\n");
                else
                    matrix.Append("]]\n");
            }

            var o = matrix.ToString();
        }

        [Test]
        public void Test_Product_Vector()
        {
            Vector x = new[] { 1, 2, 3, 4, 5, 6 };
            var result = x.Aggregate(1d, (a, i) => a *= i);
            Assert.AreEqual(1 * 2 * 3 * 4 * 5 * 6, result);
        }
    }
}
