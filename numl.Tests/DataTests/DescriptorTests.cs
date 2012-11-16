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
using numl.Data;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace numl.Tests.DataTests
{
    [TestFixture]
    public class DescriptorTests
    {
        [TestCase(-1, "Nice", typeof(bool), typeof(Property))]
        [TestCase(0, "Name", typeof(string), typeof(StringProperty))]
        [TestCase(1, "Grade", typeof(Grade), typeof(Property))]
        [TestCase(2, "GPA", typeof(double), typeof(Property))]
        [TestCase(3, "Age", typeof(int), typeof(Property))]
        [TestCase(4, "Friends", typeof(int), typeof(Property))]
        public void Test_Student_Descriptor(int index, string name, Type type, Type propertyType)
        {
            var d = Descriptor.Create<Student>();
            var property = index < 0 ? d.Label : d.Features[index];
            Assert.AreEqual(name, property.Name);
            Assert.AreEqual(type, property.Type);
            Assert.AreEqual(propertyType, property.GetType());
        }

        [TestCase(0, "Date1", DateTimeFeature.Day)]
        [TestCase(1, "Date2", DateTimeFeature.Day | DateTimeFeature.Year)]
        [TestCase(2, "Date3", DateTimeFeature.Day | DateTimeFeature.DayOfYear | DateTimeFeature.Millisecond)]
        [TestCase(3, "Date4", DateTimeFeature.Month | DateTimeFeature.Year | DateTimeFeature.Second | DateTimeFeature.Hour)]
        public void Test_Good_Date_Descriptor_With_Features(int index, string name, DateTimeFeature feature)
        {
            var desc = Descriptor.Create<FakeDate>();
            Assert.AreEqual(name, desc.Features[index].Name);
            Assert.AreEqual(new DateTimeProperty(feature).Features, ((DateTimeProperty)desc.Features[index]).Features);
        }

        [TestCase(4, "Date5", DatePortion.Date)]
        [TestCase(5, "Date6", DatePortion.Date | DatePortion.TimeExtended)]
        [TestCase(6, "Date7", DatePortion.Time)]
        [TestCase(7, "Date8", DatePortion.Date | DatePortion.DateExtended)]
        public void Test_Good_Date_Descriptor_With_Portions(int index, string name, DatePortion portion)
        {
            var desc = Descriptor.Create<FakeDate>();
            Assert.AreEqual(name, desc.Features[index].Name);
            Assert.AreEqual(new DateTimeProperty(portion).Features, ((DateTimeProperty)desc.Features[index]).Features);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Test_Bad_Date_Descriptor()
        {
            Descriptor.Create<FakeDateWithError>();
        }

        [TestCase(0, "Numbers1", 20)]
        [TestCase(1, "Numbers2", 5)]
        [TestCase(2, "Numbers3", 46)]
        public void Test_Good_Enumrable_Descriptor(int index, string name, int length)
        {
            var desc = Descriptor.Create<FakeEnumerable>();
            Assert.AreEqual(name, desc.Features[index].Name);
            Assert.AreEqual(length, ((EnumerableProperty)desc.Features[index]).Length);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Test_Bad_Enumerable_Descriptor()
        {
            Descriptor.Create<FakEnumerableWithError1>();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Test_Bad_Enumerable_0_Descriptor()
        {
            Descriptor.Create<FakEnumerableWithError2>();
        }
    }
}
