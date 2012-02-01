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
using System.Linq.Expressions;

namespace numl.Math.Optimization
{
    public class OptimizeBy
    {
        private static double MIN_DIFF = 0.0000001;
        public static Vector GradientDescent(
            Expression<Func<Vector, double>> function, 
            Expression<Func<Vector, Vector>> gradient, 
            Vector x0, 
            int iter, 
            double stepSize)
        {
            var x = x0;
            var obj = double.MaxValue;
            var f = function.Compile();
            var g = gradient.Compile();
            for (int i = 0; i < iter; i++)
            {
                
                // calculate gradient
                var grad = g(x);
                // get step size (avoid overstepping)
                var eta = stepSize / System.Math.Sqrt(iter + 1);
                // take step
                x = x - eta * grad;
                // calculate objective
                var next = f(x);
                // sanity check (don't want to take too many steps)
                if (System.Math.Abs(obj - next) < MIN_DIFF)
                    break;
                // start again
                obj = next;
            }

            return x;
        }

        public static Vector GradientDescent(
            Expression<Func<Vector, double>> function, 
            Expression<Func<Vector, Vector>> gradient, 
            Vector x0, 
            int iter, 
            double stepSize, 
            out Vector trajectory)
        {
            trajectory = Vector.Zeros(iter);
            var x = x0;
            var obj = double.MaxValue;
            var f = function.Compile();
            var g = gradient.Compile();
            for (int i = 0; i < iter; i++)
            {
                // calculate gradient
                var grad = g(x);
                // get step size (avoid overstepping)
                var eta = stepSize / System.Math.Sqrt(iter + 1);
                // take step
                x = x - eta * grad;
                // calculate objective
                trajectory[i] = f(x);
                // sanity check (don't want to take too many steps)
                if (System.Math.Abs(obj - trajectory[i]) < MIN_DIFF)
                    break;
                // start again
                obj = trajectory[i];
            }

            return x;
        }
    }
}
