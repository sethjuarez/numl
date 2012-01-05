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
using numl.Math.Information;
using NUnit.Framework;

namespace numl.Math.Tests
{
    [TestFixture]
    public class ImpurityTests
    {
        [Test]
        public void Test_Gini()
        {
            // sanity check
            Assert.AreEqual(0, Gini.Of(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }).Value);

            Vector y = new[] { 1, 1, 1, 1, 3, 3, 3, 2, 2, 2 };
            Assert.AreEqual(0.660, Gini.Of(y).Value);
        }

        [Test]
        public void Test_Gini_Conditional()
        {
            // Cheap        = 1,  Bus   = 1  
            // Expensive    = 2,  Train = 2
            // Standard     = 3,  Car   = 3
            Vector x = new[] { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };
            Vector y = new[] { 1, 1, 1, 1, 2, 3, 3, 3, 2, 2 };

            var value = Gini.Of(y).Given(x).Value;
            Assert.AreEqual(0.16, value);
        }


        [Test]
        public void Test_Gini_Information_Gain()
        {
            // Cheap        = 1,  Bus   = 1  
            // Expensive    = 2,  Train = 2
            // Standard     = 3,  Car   = 3
            Vector x = new[] { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };
            Vector y = new[] { 1, 1, 1, 1, 2, 3, 3, 3, 2, 2 };

            var value = Gini.Of(y).Given(x).Gain();
            Assert.AreEqual(0.5, value);
        }

        [Test]
        public void Test_Gini_Relative_Information_Gain()
        {
            // Math     = 1
            // History  = 2
            // CS       = 3
            Vector x = new[] { 1, 2, 3, 1, 1, 3, 2, 1 };
            Vector y = new[] { 1, 0, 1, 0, 0, 1, 0, 1 };

            var value = Entropy.Of(y).Given(x).RelativeGain();
            Assert.AreEqual(0.5, value);
        }

        [Test]
        public void Test_Entropy()
        {
            // sanity check
            Assert.AreEqual(0, Entropy.Of(new[] { 1, 1, 1, 1, 1, 1, 1, 1 }).Value);

            // Cheap        = 1,  Bus   = 1  
            // Expensive    = 2,  Train = 2
            // Standard     = 3,  Car   = 3
            Vector y = new[] { 1, 1, 1, 1, 2, 3, 3, 3, 2, 2 };

            Assert.AreEqual(1.571, Entropy.Of(y).Value);
        }

        [Test]
        public void Test_Entropy_Conditional()
        {
            // Math     = 1
            // History  = 2
            // CS       = 3
            Vector x = new[] { 1, 2, 3, 1, 1, 3, 2, 1 };
            Vector y = new[] { 1, 0, 1, 0, 0, 1, 0, 1 };

            var value = Entropy.Of(y).Given(x).Value;
            Assert.AreEqual(0.5, value);
        }


        [Test]
        public void Test_Entropy_Information_Gain()
        {
            // Math     = 1
            // History  = 2
            // CS       = 3
            Vector x = new[] { 1, 2, 3, 1, 1, 3, 2, 1 };
            Vector y = new[] { 1, 0, 1, 0, 0, 1, 0, 1 };

            var value = Entropy.Of(y).Given(x).Gain();
            Assert.AreEqual(0.5, value);
        }

        [Test]
        public void Test_Entropy_Relative_Information_Gain()
        {
            // Math     = 1
            // History  = 2
            // CS       = 3
            Vector x = new[] { 1, 2, 3, 1, 1, 3, 2, 1 };
            Vector y = new[] { 1, 0, 1, 0, 0, 1, 0, 1 };

            var value = Entropy.Of(y).Given(x).RelativeGain();
            Assert.AreEqual(0.5, value);
        }


        [Test]
        public void Test_Error()
        {
            // sanity check
            Assert.AreEqual(0, Error.Of(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }).Value);

            Vector y = new[] { 1, 1, 1, 1, 3, 3, 3, 2, 2, 2 };
            Assert.AreEqual(0.600, Error.Of(y).Value);
        }

        [Test]
        public void Test_Error_Conditional()
        {
            // Cheap        = 1,  Bus   = 1  
            // Expensive    = 2,  Train = 2
            // Standard     = 3,  Car   = 3
            Vector x = new[] { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };
            Vector y = new[] { 1, 1, 1, 1, 2, 3, 3, 3, 2, 2 };

            var value = Error.Of(y).Given(x).Value;
            Assert.AreEqual(0.1, value);
        }


        [Test]
        public void Test_Error_Information_Gain()
        {
            // Cheap        = 1,  Bus   = 1  
            // Expensive    = 2,  Train = 2
            // Standard     = 3,  Car   = 3
            Vector x = new[] { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };
            Vector y = new[] { 1, 1, 1, 1, 2, 3, 3, 3, 2, 2 };

            var value = Error.Of(y).Given(x).Gain();
            Assert.AreEqual(0.5, value);
        }

        [Test]
        public void Test_Error_Relative_Information_Gain()
        {
            // Math     = 1
            // History  = 2
            // CS       = 3
            Vector x = new[] { 1, 2, 3, 1, 1, 3, 2, 1 };
            Vector y = new[] { 1, 0, 1, 0, 0, 1, 0, 1 };

            var value = Error.Of(y).Given(x).RelativeGain();
            Assert.AreEqual(0.5, value);
        }

        [Test]
        public void Test_Entropy_Tennis()
        {
            // Outlook
            // Sunny = 1
            // Overcast = 2
            // Rainy = 3
            Vector outlook = new[] { 1, 1, 1, 2, 2, 2, 3, 3 };
            Vector windy   = new[] { 1, 1, 0, 1, 0, 0, 1, 0 };
            Vector temp    = new[] { 0, 1, 1, 0, 1, 0, 0, 0 };

            Vector playing = new[] { 1, 0, 0, 1, 1, 1, 0, 1 };

            var y = Entropy.Of(playing).Value;
            var y_otlk = Entropy.Of(playing).Given(outlook).Value;
            var y_wind = Entropy.Of(playing).Given(windy).Value;
            var y_temp = Entropy.Of(playing).Given(temp).Value;

            var b_otlk = Entropy.Of(playing).Given(outlook).RelativeGain();
            var b_wind = Entropy.Of(playing).Given(windy).RelativeGain();
            var b_temp = Entropy.Of(playing).Given(temp).RelativeGain();
        }

        [Test]
        public void Test_Entropy_Concrete()
        {
            Vector x = new[] { 1, 2, 3, 4};
            var entropy = Entropy.Of(x).Value;

            x = Vector.Rand(20000);
            entropy = Entropy.Of(x).Value;

        }
    }
}
