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
using numl.Math;
using NUnit.Framework;

namespace numl.Math.Tests
{
    /// <summary>
    /// Summary description for CalculusTests
    /// </summary>
    [TestFixture]
    public class CalculusTests
    {
        [Test]
        public void GradientDescent_Trajectory_Test()
        {
            Vector trajectory;
            var opt = OptimizeBy.GradientDescent(
                // vector equivalent of X^2
                x => Vector.Dot(x, x),
                x => 2 * x,
                new Vector(new double[] { 10, 5 }),
                200,
                0.5,
                out trajectory);

            var n = Vector.Norm(opt);
            double d = double.MaxValue;
            for (int i = 199; i > -1; i--)
            {
                if (trajectory[i] > 0)
                {
                    d = trajectory[i];
                    break;
                }
            }

            //var d = trajectory[199]; // <= 0.0000001;
        }
    }
}
