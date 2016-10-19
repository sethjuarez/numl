using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using System.Collections.Generic;
using FluentAssertions.Collections;
using Xunit;
using FluentAssertions;

namespace numl.Tests.DataTests
{
    [Trait("Category", "Data")]
    public class DescriptorTests
    {
        [Theory]
        [InlineData(-1, "Nice", typeof(bool), typeof(Property))]
        [InlineData(0, "Name", typeof(string), typeof(StringProperty))]
        [InlineData(1, "Grade", typeof(Grade), typeof(Property))]
        [InlineData(2, "GPA", typeof(double), typeof(Property))]
        [InlineData(3, "Age", typeof(int), typeof(Property))]
        [InlineData(4, "Friends", typeof(int), typeof(Property))]
        public void Test_Student_Descriptor(int index, string name, Type type, Type propertyType)
        {
            var d = Descriptor.Create<Student>();
            Property property;
            if (index < 0) property = d.Label;
            else
            {

                var q = d.Features.Where(p => p.Name == name);
                q.Should().ContainSingle();
                property = q.First();
            }

            Assert.Equal(name, property.Name);
            Assert.Equal(type, property.Type);
            Assert.Equal(propertyType, property.GetType());
        }

        [Theory]
        [InlineData(0, "Date1", DateTimeFeature.Day)]
        [InlineData(1, "Date2", DateTimeFeature.Day | DateTimeFeature.Year)]
        [InlineData(2, "Date3", DateTimeFeature.Day | DateTimeFeature.DayOfYear | DateTimeFeature.Millisecond)]
        [InlineData(3, "Date4", DateTimeFeature.Month | DateTimeFeature.Year | DateTimeFeature.Second | DateTimeFeature.Hour)]
        public void Test_Good_Date_Descriptor_With_Features(int index, string name, DateTimeFeature feature)
        {
            var desc = Descriptor.Create<FakeDate>();
            Assert.Equal(name, desc.Features[index].Name);
            Assert.Equal(new DateTimeProperty(feature).Features, ((DateTimeProperty)desc.Features[index]).Features);
        }

        [Fact]
        public void Test_Good_Date_Descriptor_With_Features_Fluent()
        {
            DateTimeFeature portion = DateTimeFeature.Month | DateTimeFeature.Year | DateTimeFeature.Second | DateTimeFeature.Hour;
            var d = Descriptor.For<FakeDate>()
                              .WithDateTime(f => f.Date1, portion);

            Assert.Equal("Date1", d.Features[0].Name);
            Assert.Equal(new DateTimeProperty(portion).Features, ((DateTimeProperty)d.Features[0]).Features);
        }

        [Theory]
        [InlineData(4, "Date5", DatePortion.Date)]
        [InlineData(5, "Date6", DatePortion.Date | DatePortion.TimeExtended)]
        [InlineData(6, "Date7", DatePortion.Time)]
        [InlineData(7, "Date8", DatePortion.Date | DatePortion.DateExtended)]
        public void Test_Good_Date_Descriptor_With_Portions(int index, string name, DatePortion portion)
        {
            var desc = Descriptor.Create<FakeDate>();
            Assert.Equal(name, desc.Features[index].Name);
            Assert.Equal(new DateTimeProperty(portion).Features, ((DateTimeProperty)desc.Features[index]).Features);
        }

        [Fact]
        public void Test_Good_Date_Descriptor_With_Portions_Fluent()
        {
            DatePortion portion = DatePortion.Date | DatePortion.DateExtended;
            var d = Descriptor.For<FakeDate>()
                              .WithDateTime(f => f.Date1, portion);

            Assert.Equal("Date1", d.Features[0].Name);
            Assert.Equal(new DateTimeProperty(portion).Features, ((DateTimeProperty)d.Features[0]).Features);
        }

        [Fact]
        public void Test_Bad_Date_Descriptor()
        {
            Assert.Throws<InvalidOperationException>(() => Descriptor.Create<FakeDateWithError>());
        }
		
		[Fact]
		public void Test_Good_Guid_Descriptor()
		{
			var desc = Descriptor.Create<FakeGuid>();
			Assert.Equal("Guid1", desc.Features[0].Name);
		}

		[Fact]
		public void Test_Good_Guid_Descriptor_Fluent()
		{
			var d = Descriptor.For<FakeGuid>()
							  .WithGuid(f => f.Guid1);

			Assert.Equal("Guid1", d.Features[0].Name);
		}
		
		[Fact]
		public void Test_Bad_Guid_Descriptor()
		{
			Assert.Throws<InvalidOperationException>(() => Descriptor.Create<FakeGuidWithError>());
		}

		[Theory]
        [InlineData(0, "Numbers1", 20)]
        [InlineData(1, "Numbers2", 5)]
        [InlineData(2, "Numbers3", 46)]
        public void Test_Good_Enumerable_Descriptor(int index, string name, int length)
        {
            var desc = Descriptor.Create<FakeEnumerable>();
            Assert.Equal(name, desc.Features[index].Name);
            Assert.Equal(length, ((EnumerableProperty)desc.Features[index]).Length);
        }


        [Fact]
        public void Test_Good_Enumerable_Fluent()
        {
            var length = 10;
            var d = Descriptor.For<FakeEnumerable>()
                               .WithEnumerable(e => e.Numbers1, length);

            Assert.Equal("Numbers1", d.Features[0].Name);
            Assert.Equal(length, ((EnumerableProperty)d.Features[0]).Length);
        }


        [Fact]
        public void Test_Bad_Enumerable_Descriptor()
        {
            Assert.Throws<InvalidOperationException>(
                () => Descriptor.Create<FakEnumerableWithError1>());
        }

        [Fact]
        public void Test_Bad_Enumerable_0_Descriptor()
        {
            Assert.Throws<InvalidOperationException>(
                () => Descriptor.Create<FakEnumerableWithError2>());
        }

        [Fact]
        public void Test_Fluent_Api()
        {
            var d = Descriptor.New()
                        .With("SepalLength").As(typeof(decimal))
                        .With("SepalWidth").As(typeof(double))
                        .With("PetalLength").As(typeof(decimal))
                        .With("PetalWidth").As(typeof(int))
                        .Learn("Class").As(typeof(string));

            Assert.Equal(4, d.Features.Length);
            Assert.Equal(typeof(decimal), d.Features[0].Type);
            Assert.Equal(typeof(double), d.Features[1].Type);
            Assert.Equal(typeof(decimal), d.Features[2].Type);
            Assert.Equal(typeof(int), d.Features[3].Type);
            Assert.Equal("Class", d.Label.Name);
        }

        [Fact]
        public void Test_Typed_Fluent_Api()
        {
            var d = Descriptor.For<Iris>()
                                .With(i => i.SepalLength)
                                .With(i => i.SepalWidth)
                                .With(i => i.PetalLength)
                                .With(i => i.PetalWidth)
                                .Learn(i => i.Class);

            Assert.Equal(4, d.Features.Length);
            Assert.Equal(typeof(decimal), d.Features[0].Type);
            Assert.Equal(typeof(decimal), d.Features[1].Type);
            Assert.Equal(typeof(decimal), d.Features[2].Type);
            Assert.Equal(typeof(decimal), d.Features[3].Type);
            Assert.Equal("Class", d.Label.Name);
            Assert.Equal(1, d.Label.Length);
        }

        [Fact]
        public void Test_Attributes_Api()
        {
            var d = Descriptor.Create<Iris>();

            Assert.Equal(4, d.Features.Length);
            Assert.Equal(typeof(decimal), d.Features[0].Type);
            Assert.Equal(typeof(decimal), d.Features[1].Type);
            Assert.Equal(typeof(decimal), d.Features[2].Type);
            Assert.Equal(typeof(decimal), d.Features[3].Type);
            Assert.Equal("Class", d.Label.Name);
            Assert.Equal(1, d.Label.Length);
        }
    }
}
