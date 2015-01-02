using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using numl.Tests.Data;
using System.Collections.Generic;

namespace numl.Tests.DataTests
{
    [TestFixture, Category("Data")]
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

        [Test]
        public void Test_Good_Date_Descriptor_With_Features_Fluent()
        {
            DateTimeFeature portion = DateTimeFeature.Month | DateTimeFeature.Year | DateTimeFeature.Second | DateTimeFeature.Hour;
            var d = Descriptor.For<FakeDate>()
                              .WithDateTime(f => f.Date1, portion);

            Assert.AreEqual("Date1", d.Features[0].Name);
            Assert.AreEqual(new DateTimeProperty(portion).Features, ((DateTimeProperty)d.Features[0]).Features);
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

        [Test]
        public void Test_Good_Date_Descriptor_With_Portions_Fluent()
        {
            DatePortion portion = DatePortion.Date | DatePortion.DateExtended;
            var d = Descriptor.For<FakeDate>()
                              .WithDateTime(f => f.Date1, portion);

            Assert.AreEqual("Date1", d.Features[0].Name);
            Assert.AreEqual(new DateTimeProperty(portion).Features, ((DateTimeProperty)d.Features[0]).Features);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Test_Bad_Date_Descriptor()
        {
            Descriptor.Create<FakeDateWithError>();
        }

        [TestCase(0, "Numbers1", 20)]
        [TestCase(1, "Numbers2", 5)]
        [TestCase(2, "Numbers3", 46)]
        public void Test_Good_Enumerable_Descriptor(int index, string name, int length)
        {
            var desc = Descriptor.Create<FakeEnumerable>();
            Assert.AreEqual(name, desc.Features[index].Name);
            Assert.AreEqual(length, ((EnumerableProperty)desc.Features[index]).Length);
        }


        [Test]
        public void Test_Good_Enumerable_Fluent()
        {
            var length = 10;
            var d = Descriptor.For<FakeEnumerable>()
                               .WithEnumerable(e => e.Numbers1, length);

            Assert.AreEqual("Numbers1", d.Features[0].Name);
            Assert.AreEqual(length, ((EnumerableProperty)d.Features[0]).Length);
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

        [Test]
        public void Test_Fluent_Api()
        {
            var d = Descriptor.New()
                        .With("SepalLength").As(typeof(decimal))
                        .With("SepalWidth").As(typeof(double))
                        .With("PetalLength").As(typeof(decimal))
                        .With("PetalWidth").As(typeof(int))
                        .Learn("Class").As(typeof(string));

            Assert.AreEqual(4, d.Features.Length);
            Assert.AreEqual(typeof(decimal), d.Features[0].Type);
            Assert.AreEqual(typeof(double), d.Features[1].Type);
            Assert.AreEqual(typeof(decimal), d.Features[2].Type);
            Assert.AreEqual(typeof(int), d.Features[3].Type);
            Assert.AreEqual("Class", d.Label.Name);
        }

      [Test]
        public void Test_Typed_Fluent_Api()
        {
            var d = Descriptor.For<Iris>()
                                .With(i => i.SepalLength)
                                .With(i => i.SepalWidth)
                                .With(i => i.PetalLength)
                                .With(i => i.PetalWidth)
                                .Learn(i => i.Class);
          
            Assert.AreEqual(4, d.Features.Length);
            Assert.AreEqual(typeof(decimal), d.Features[0].Type);
            Assert.AreEqual(typeof(decimal), d.Features[1].Type);
            Assert.AreEqual(typeof(decimal), d.Features[2].Type);
            Assert.AreEqual(typeof(decimal), d.Features[3].Type);
            Assert.AreEqual("Class", d.Label.Name);           
            Assert.AreEqual(1, d.Label.Length);           
        }

      [Test]
      public void Test_Attributes_Api()
      {
          var d = Descriptor.Create<Iris>();

          Assert.AreEqual(4, d.Features.Length);
          Assert.AreEqual(typeof(decimal), d.Features[0].Type);
          Assert.AreEqual(typeof(decimal), d.Features[1].Type);
          Assert.AreEqual(typeof(decimal), d.Features[2].Type);
          Assert.AreEqual(typeof(decimal), d.Features[3].Type);
          Assert.AreEqual("Class", d.Label.Name);
          Assert.AreEqual(1, d.Label.Length);           
      }
    }
}
