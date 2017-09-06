using System;
using numl.Model;
using numl.Utils;
using System.Linq;
using numl.Tests.Data;
using Xunit;
using System.Collections.Generic;
using numl.Serialization;

namespace numl.Tests.SerializationTests.DescriptorSerialization
{
	[Trait("Category", "Serialization")]
	public class DescriptorSerializationTests : BaseSerialization
	{
		public static IEnumerable<string> WordStrings
		{
			get
			{
				yield return "TH@e QUi#CK bRow!N fox 12";
				yield return "s@upeR B#roWN BEaR";
				yield return "1829! UGlY FoX";
				yield return "ThE QuICk beAr";
			}
		}

		[Fact]
		public void Descriptor_Save_And_Load()
		{
			var data = Iris.Load();
			var description = Descriptor.Create<Iris>();
			// to populate dictionaries
			var examples = description.ToExamples(data);

            var file = GetPath();
            Register.Type<Iris>();
            var d = SaveAndLoad(description, file);

			Assert.Equal(description.Type, d.Type);
			Assert.Equal(description.Features.Length, d.Features.Length);
			for (int i = 0; i < description.Features.Length; i++)
			{
				Assert.Equal(description.Features[i].Type, d.Features[i].Type);
				Assert.Equal(description.Features[i].Name, d.Features[i].Name);
				Assert.Equal(description.Features[i].Start, d.Features[i].Start);
			}

			Assert.Equal(description.Label.Type, d.Label.Type);
			Assert.Equal(description.Label.Name, d.Label.Name);
			Assert.Equal(description.Label.Start, d.Label.Start);
		}

        [Fact]
        public void Descriptor_Save_And_Load_Json()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            // to populate dictionaries
            var examples = description.ToExamples(data);

            var file = GetPath();
            Register.Type<Iris>();
            var d = SaveAndLoadJson(description);

            Assert.Equal(description.Type, d.Type);
            Assert.Equal(description.Features.Length, d.Features.Length);
            for (int i = 0; i < description.Features.Length; i++)
            {
                Assert.Equal(description.Features[i].Type, d.Features[i].Type);
                Assert.Equal(description.Features[i].Name, d.Features[i].Name);
                Assert.Equal(description.Features[i].Start, d.Features[i].Start);
            }

            Assert.Equal(description.Label.Type, d.Label.Type);
            Assert.Equal(description.Label.Name, d.Label.Name);
            Assert.Equal(description.Label.Start, d.Label.Start);
        }

        [Fact]
		public void Complex_Descriptor_Save_And_Load_No_Label()
		{
			var data = Generic.GetRows(100).ToArray();
			var description = Descriptor.Create<Generic>();
			// to populate dictionaries
			var examples = description.ToExamples(data);

            var file = GetPath();
            Register.Type<Generic>();
            var d = SaveAndLoad(description, file);

            Assert.Equal(description.Type, d.Type);
			Assert.Equal(description.Features.Length, d.Features.Length);
			for (int i = 0; i < description.Features.Length; i++)
			{
				Assert.Equal(description.Features[i].Type, d.Features[i].Type);
				Assert.Equal(description.Features[i].Name, d.Features[i].Name);
				Assert.Equal(description.Features[i].Start, d.Features[i].Start);
			}

			Assert.True(d.Label == null);
		}

        [Fact]
        public void Complex_Descriptor_Save_And_Load_No_Label_Json()
        {
            var data = Generic.GetRows(100).ToArray();
            var description = Descriptor.Create<Generic>();
            // to populate dictionaries
            var examples = description.ToExamples(data);

            var file = GetPath();
            Register.Type<Generic>();
            var d = SaveAndLoadJson(description);

            Assert.Equal(description.Type, d.Type);
            Assert.Equal(description.Features.Length, d.Features.Length);
            for (int i = 0; i < description.Features.Length; i++)
            {
                Assert.Equal(description.Features[i].Type, d.Features[i].Type);
                Assert.Equal(description.Features[i].Name, d.Features[i].Name);
                Assert.Equal(description.Features[i].Start, d.Features[i].Start);
            }

            Assert.True(d.Label == null);
        }


        [Fact]
		public void Descriptor_Dictionary_Serialization_Test()
		{
			Descriptor description = new Descriptor();

			var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);

			description.Features = new Property[]
			{
				new Property { Name = "Age", Type = typeof(int) },
				new Property { Name = "Height", Type = typeof(decimal) },
				new StringProperty { Name = "Words", Dictionary = dictionary },
				new Property { Name = "Weight", Type = typeof(double) },
				new Property { Name = "Good", Type = typeof(bool) },
				
			};

            var file = GetPath();
            var d = SaveAndLoad(description, file);

            Assert.Equal(description, d);
		}

        [Fact]
        public void Descriptor_Dictionary_Serialization_Test_Json()
        {
            Descriptor description = new Descriptor();

            var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);

            description.Features = new Property[]
            {
                new Property { Name = "Age", Type = typeof(int) },
                new Property { Name = "Height", Type = typeof(decimal) },
                new StringProperty { Name = "Words", Dictionary = dictionary },
                new Property { Name = "Weight", Type = typeof(double) },
                new Property { Name = "Good", Type = typeof(bool) },

            };

            var file = GetPath();
            var d = SaveAndLoadJson(description);

            Assert.Equal(description, d);
        }

        [Fact]
		public void Descriptor_Full_Serialization_Test()
		{
			var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);
			var guidCategories = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };

			Descriptor description = Descriptor.New("MyDescriptor")
									 .With("NumberProp").As(typeof(double))
									 .With("EnumerableProp").AsEnumerable(5)
									 .With("DateTimeProp").AsDateTime(DatePortion.Date | DatePortion.Time)
									 .With("GuidProp").AsGuid()
									 .With("StringProp").AsString(StringSplitType.Word)
									 .With("StringEnumProp").AsStringEnum()
									 .Learn("TargetProp").As(typeof(bool));

			((StringProperty)description["StringProp"]).Dictionary = dictionary;
			((StringProperty)description["StringEnumProp"]).Dictionary = dictionary;
			((GuidProperty)description["GuidProp"]).Categories = guidCategories;

            var file = GetPath();
            var d = SaveAndLoad(description, file);

            Assert.Equal(description, d);
		}

        [Fact]
        public void Descriptor_Full_Serialization_Test_Json()
        {
            var dictionary = StringHelpers.BuildDistinctWordArray(WordStrings);
            var guidCategories = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };

            Descriptor description = Descriptor.New("MyDescriptor")
                                     .With("NumberProp").As(typeof(double))
                                     .With("EnumerableProp").AsEnumerable(5)
                                     .With("DateTimeProp").AsDateTime(DatePortion.Date | DatePortion.Time)
                                     .With("GuidProp").AsGuid()
                                     .With("StringProp").AsString(StringSplitType.Word)
                                     .With("StringEnumProp").AsStringEnum()
                                     .Learn("TargetProp").As(typeof(bool));

            ((StringProperty)description["StringProp"]).Dictionary = dictionary;
            ((StringProperty)description["StringEnumProp"]).Dictionary = dictionary;
            ((GuidProperty)description["GuidProp"]).Categories = guidCategories;

            var file = GetPath();
            var d = SaveAndLoadJson(description);

            Assert.Equal(description, d);
        }

        [Fact]
		public void Descriptor_With_Class_Serialization_Test()
		{
			var description = Descriptor.Create<House>();

            var file = GetPath();
            Register.Type<House>();
            var d = SaveAndLoad(description, file);

            Assert.Equal(description, d);
		}

        [Fact]
        public void Descriptor_With_Class_Serialization_Test_Json()
        {
            var description = Descriptor.Create<House>();

            var file = GetPath();
            Register.Type<House>();
            var d = SaveAndLoadJson(description);

            Assert.Equal(description, d);
        }
    }
}
