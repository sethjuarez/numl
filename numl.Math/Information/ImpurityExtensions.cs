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

namespace numl.Math.Information
{
    public static class ImpurityExtensions
    {
        public static Entropy Given(this Entropy e, Vector x)
        {
            if (e._x != null)
                return new Entropy { _x = x, _y = e._x, _conditional = true };
            else
                throw new InvalidOperationException("Cannot Invoke Given Clause Given Empty Operand!");
        }

        public static Entropy WithWidth(this Entropy e, int width)
        {
            e._width = width;
            return e;
        }

        public static Gini Given(this Gini e, Vector x)
        {
            if (e._x != null)
                return new Gini { _x = x, _y = e._x, _conditional = true };
            else
                throw new InvalidOperationException("Cannot Invoke Given Clause Given Empty Operand!");
        }

        public static Gini WithWidth(this Gini e, int width)
        {
            e._width = width;
            return e;
        }

        public static Error Given(this Error e, Vector x)
        {
            if (e._x != null)
                return new Error { _x = x, _y = e._x, _conditional = true };
            else
                throw new InvalidOperationException("Cannot Invoke Given Clause Given Empty Operand!");
        }

        public static Error WithWidth(this Error e, int width)
        {
            e._width = width;
            return e;
        }


        public static double Entropy(this Vector x)
        {
            return numl.Math.Information.Entropy.Of(x).Value;
        }

        public static double Gini(this Vector x)
        {
            return numl.Math.Information.Gini.Of(x).Value;
        }
    }
}
