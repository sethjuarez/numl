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

using numl.Data;
using numl.Model;
using numl.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Tests.DataTests
{
    [TestFixture]
    public class DescriptorTests
    {
        [Test]
        public void Test_Student_Descriptor()
        {
            var d = Descriptor.Create<Student>();
            Assert.AreEqual(5, d.Features.Length);
            
            Assert.AreEqual("Name", d.Features[0].Name);
            Assert.AreEqual(typeof(string), d.Features[0].Type);
            Assert.AreEqual(typeof(StringProperty), d.Features[0].GetType());

            Assert.AreEqual("Grade", d.Features[1].Name);
            Assert.AreEqual(typeof(Grade), d.Features[1].Type);

            Assert.AreEqual("GPA", d.Features[2].Name);
            Assert.AreEqual(typeof(double), d.Features[2].Type);

            Assert.AreEqual("Age", d.Features[3].Name);
            Assert.AreEqual(typeof(int), d.Features[3].Type);

            Assert.AreEqual("Friends", d.Features[4].Name);
            Assert.AreEqual(typeof(int), d.Features[4].Type);

            Assert.AreEqual("Nice", d.Label.Name);
            Assert.AreEqual(typeof(bool), d.Label.Type);
        }
    }
}
